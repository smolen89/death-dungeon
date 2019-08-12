using System;
using System.Collections.Generic;

namespace RD.ECS.Events_Tiny
{
	public class EventManager : IEventManager
	{
		//todo comments
		protected List<ListenerEntry> listeners;
		protected Stack<int> freeEntriesRegistry;

		public EventManager()
		{
			listeners = new List<ListenerEntry>();
			freeEntriesRegistry = new Stack<int>();
		}

		public uint RegisterListener<TEventData>( IEventListener eventListener ) where TEventData : struct
		{
			// Sprawdzić czy podany eventListener nie ma wartości null
			if( eventListener == null )
			{
				throw new ArgumentNullException( nameof( eventListener ) );
			}

			// Cza znaleźć pierwszy wolny index/wpis w tabeli listenerów
			int firstFreeEntryIndex = freeEntriesRegistry.Count > 0 ? freeEntriesRegistry.Pop() : listeners.Count;

			// jeśli index jest przypisany z Stacka to w listener będzie brakowało ilościowo wpisów,
			// dlatego cza ten wpis będzie dodać do listenera. Narazie jako pusty.
			//hack niewiem dlaczego akurat w ten sposów to rozwiązano ale mi się to nie podoba!!!

			if( firstFreeEntryIndex >= listeners.Count )
			{
				listeners.Add( new ListenerEntry { } );
			}

			listeners[ firstFreeEntryIndex ] = new ListenerEntry()
			{
				EventType = typeof( TEventData ),
				Listener = eventListener
			};
			return (uint)firstFreeEntryIndex;
		}

		public void UnregisterListener( uint listenerID )
		{
			if( listenerID >= listeners.Count )
			{
				throw new ListenerDoesntExistException( listenerID );
			}

			listeners[ (int)listenerID ] = new ListenerEntry() { };

			freeEntriesRegistry.Push( (int)listenerID );
		}

		public void Notify<TEventData>( TEventData eventData, uint destinationListenerID = uint.MaxValue ) where TEventData : struct
		{
			Type currentEventType = typeof( TEventData );
			ListenerEntry currentListenerEntry;

			if( destinationListenerID != uint.MaxValue )
			{
				if( destinationListenerID >= listeners.Count )
				{
					throw new ArgumentOutOfRangeException( nameof( destinationListenerID ) );
				}

				currentListenerEntry = listeners[ (int)destinationListenerID ];

				if( currentListenerEntry.EventType == currentEventType )
				{
					( currentListenerEntry.Listener as IEventListener<TEventData> )?.Raise( eventData );
				}
				return;
			}

			// Brodcasting to all listeners
			for( int i = 0; i < listeners.Count; i++ )
			{
				currentListenerEntry = listeners[ i ];

				if( currentListenerEntry.EventType != null && currentListenerEntry.EventType == currentEventType && currentListenerEntry.Listener != null )
				{
					( currentListenerEntry.Listener as IEventListener<TEventData> )?.Raise( eventData );
				}
			}
		}
	}
}