// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
#define LOG_ALL_MESSAGES
#define LOG_ADD_LISTENER
#define LOG_BROADCAST_MESSAGE
#define REQUIRE_LISTENER

using System;
using System.Collections.Generic;
using UnityEngine;

internal static class Messenger
{
	#region Internal variables

	//Disable the unused variable warning
#pragma warning disable 0414

	//Ensures that the MessengerHelper will be created automatically upon start of the game.
	private static MessengerComponent messengerHelper = ( new GameObject("MessengerHelper") ).AddComponent< MessengerComponent >();

#pragma warning restore 0414

	public static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	//Message handlers that should never be removed, regardless of calling Cleanup
	public static List<string> permanentMessages = new List<string> ();

	#endregion Internal variables

	#region Helper methods

	//Marks a certain message as permanent.
	public static void MarkAsPermanent( string eventType )
	{
#if LOG_ALL_MESSAGES
		Debugger.Log( "Messenger", $"MarkAsPermanent() -- \t'{eventType}'" );
#endif

		permanentMessages.Add( eventType );
	}

	public static void Cleanup()
	{
#if LOG_ALL_MESSAGES
		Debugger.LogInfo( "Messenger", $"Cleanup() -- Make sure that none of necessary listeners are removed." );
#endif

		List<string> messagesToRemove = new List<string>();

		foreach( KeyValuePair<string, Delegate> pair in eventTable )
		{
			bool wasFound = false;

			foreach( string message in permanentMessages )
			{
				if( pair.Key == message )
				{
					wasFound = true;
					break;
				}
			}

			if( !wasFound )
				messagesToRemove.Add( pair.Key );
		}

		foreach( string message in messagesToRemove )
		{
			eventTable.Remove( message );
		}
	}

	public static void PrintEventTable()
	{
		Debugger.Log( "Messenger", $"PrintEventTable: " );
		Debugger.Log( "\t\t\t=== Table ===" );
		foreach( KeyValuePair<string, Delegate> pair in eventTable )
		{
			Debugger.Log( $"\t\t\t{pair.Key}\t\t{pair.Value}" );
		}

		Debugger.Log( "\n" );
	}

	#endregion Helper methods

	#region Message logging and exception throwing

	static public void OnListenerAdding( string eventType, Delegate listenerBeingAdded )
	{
#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
		Debugger.Log( "Messenger", $"OnListenerAdding() -- '{eventType}'\t[{listenerBeingAdded.Target} -> {listenerBeingAdded.Method}]" );
#endif

		if( !eventTable.ContainsKey( eventType ) )
		{
			eventTable.Add( eventType, null );
		}

		Delegate d = eventTable[eventType];
		if( d != null && d.GetType() != listenerBeingAdded.GetType() )
		{
			throw new ListenerException( string.Format( "Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name ) );
		}
	}

	static public void OnListenerRemoving( string eventType, Delegate listenerBeingRemoved )
	{
#if LOG_ALL_MESSAGES
		Debugger.Log( "Messenger", " OnListenerRemoving() --  \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}" );
#endif

		if( eventTable.ContainsKey( eventType ) )
		{
			Delegate d = eventTable[eventType];

			if( d == null )
			{
				throw new ListenerException( string.Format( "Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType ) );
			}
			else if( d.GetType() != listenerBeingRemoved.GetType() )
			{
				throw new ListenerException( string.Format( "Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name ) );
			}
		}
		else
		{
			throw new ListenerException( string.Format( "Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType ) );
		}
	}

	static public void OnListenerRemoved( string eventType )
	{
		if( eventTable[ eventType ] == null )
		{
			eventTable.Remove( eventType );
		}
	}

	static public void OnBroadcasting( string eventType )
	{
#if REQUIRE_LISTENER
		if( !eventTable.ContainsKey( eventType ) )
		{
			throw new BroadcastException( string.Format( "Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType ) );
		}
#endif
	}

	static public BroadcastException CreateBroadcastSignatureException( string eventType )
	{
		return new BroadcastException( string.Format( "Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType ) );
	}

	public class BroadcastException : Exception
	{
		public BroadcastException( string msg )
			: base( msg )
		{
		}
	}

	public class ListenerException : Exception
	{
		public ListenerException( string msg )
			: base( msg )
		{
		}
	}

	#endregion Message logging and exception throwing

	#region AddListener

	//No parameters
	static public void AddListener( string eventType, Callback handler )
	{
		OnListenerAdding( eventType, handler );
		eventTable[ eventType ] = (Callback)eventTable[ eventType ] + handler;
	}

	//Single parameter
	static public void AddListener<T>( string eventType, Callback<T> handler )
	{
		OnListenerAdding( eventType, handler );
		eventTable[ eventType ] = (Callback<T>)eventTable[ eventType ] + handler;
	}

	//Two parameters
	static public void AddListener<T, U>( string eventType, Callback<T, U> handler )
	{
		OnListenerAdding( eventType, handler );
		eventTable[ eventType ] = (Callback<T, U>)eventTable[ eventType ] + handler;
	}

	//Three parameters
	static public void AddListener<T, U, V>( string eventType, Callback<T, U, V> handler )
	{
		OnListenerAdding( eventType, handler );
		eventTable[ eventType ] = (Callback<T, U, V>)eventTable[ eventType ] + handler;
	}

	#endregion AddListener

	#region RemoveListener

	//No parameters
	static public void RemoveListener( string eventType, Callback handler )
	{
		OnListenerRemoving( eventType, handler );
		eventTable[ eventType ] = (Callback)eventTable[ eventType ] - handler;
		OnListenerRemoved( eventType );
	}

	//Single parameter
	static public void RemoveListener<T>( string eventType, Callback<T> handler )
	{
		OnListenerRemoving( eventType, handler );
		eventTable[ eventType ] = (Callback<T>)eventTable[ eventType ] - handler;
		OnListenerRemoved( eventType );
	}

	//Two parameters
	static public void RemoveListener<T, U>( string eventType, Callback<T, U> handler )
	{
		OnListenerRemoving( eventType, handler );
		eventTable[ eventType ] = (Callback<T, U>)eventTable[ eventType ] - handler;
		OnListenerRemoved( eventType );
	}

	//Three parameters
	static public void RemoveListener<T, U, V>( string eventType, Callback<T, U, V> handler )
	{
		OnListenerRemoving( eventType, handler );
		eventTable[ eventType ] = (Callback<T, U, V>)eventTable[ eventType ] - handler;
		OnListenerRemoved( eventType );
	}

	#endregion RemoveListener

	#region Broadcast

	//No parameters
	static public void Broadcast( string eventType )
	{
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debugger.Log( "Messenger", "Broadcast\t\t\tInvoking \t\"" + eventType + "\"" );
#endif
		OnBroadcasting( eventType );

		if( eventTable.TryGetValue( eventType, out Delegate d ) )
		{

			if( d is Callback callback )
			{
				callback();
			}
			else
			{
				throw CreateBroadcastSignatureException( eventType );
			}
		}
	}

	//Single parameter
	static public void Broadcast<T>( string eventType, T arg1 )
	{
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debugger.Log( "Messenger", "Broadcast\t\t\tInvoking \t\"" + eventType + "\"" );
#endif
		OnBroadcasting( eventType );

		if( eventTable.TryGetValue( eventType, out Delegate d ) )
		{

			if( d is Callback<T> callback )
			{
				callback( arg1 );
			}
			else
			{
				throw CreateBroadcastSignatureException( eventType );
			}
		}
	}

	//Two parameters
	static public void Broadcast<T, U>( string eventType, T arg1, U arg2 )
	{
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debugger.Log( "Messenger", "Broadcast\t\t\tInvoking \t\"" + eventType + "\"" );
#endif
		OnBroadcasting( eventType );

		if( eventTable.TryGetValue( eventType, out Delegate d ) )
		{

			if( d is Callback<T, U> callback )
			{
				callback( arg1, arg2 );
			}
			else
			{
				throw CreateBroadcastSignatureException( eventType );
			}
		}
	}

	//Three parameters
	static public void Broadcast<T, U, V>( string eventType, T arg1, U arg2, V arg3 )
	{
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debugger.Log( "Messenger", "Broadcast\t\t\tInvoking \t\"" + eventType + "\"" );
#endif
		OnBroadcasting( eventType );

		if( eventTable.TryGetValue( eventType, out Delegate d ) )
		{

			if( d is Callback<T, U, V> callback )
			{
				callback( arg1, arg2, arg3 );
			}
			else
			{
				throw CreateBroadcastSignatureException( eventType );
			}
		}
	}

	#endregion Broadcast
}
