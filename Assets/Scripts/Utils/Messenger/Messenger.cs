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
	private const string MessengerHelperGameObjectName = "MessengerHelper";
	private const string LogChannelName = "Messenger";

	#region Internal variables

	//Disable the unused variable warning
	#pragma warning disable 0414

	//Ensures that the MessengerHelper will be created automatically upon start of the game.
	// ReSharper disable once UnusedMember.Local
	private static MessengerComponent _messengerHelper =
		( new GameObject( MessengerHelperGameObjectName ) ).AddComponent<MessengerComponent>();

	#pragma warning restore 0414

	private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	//Message handlers that should never be removed, regardless of calling Cleanup
	private static List<string> permanentMessages = new List<string>();

	#endregion Internal variables

	#region Helper methods

	//Marks a certain message as permanent.
	public static void MarkAsPermanent( string eventType )
	{
		#if LOG_ALL_MESSAGES
		Debugger.Log( LogChannelName, $"MarkAsPermanent() -- \t'{eventType}'" );
		#endif

		permanentMessages.Add( eventType );
	}

	public static void Cleanup()
	{
		#if LOG_ALL_MESSAGES
		Debugger.LogInfo( LogChannelName, $"Cleanup() -- Make sure that none of necessary listeners are removed." );
		#endif

		var messagesToRemove = new List<string>();

		foreach ( var pair in eventTable )
		{
			var wasFound = false;

			foreach ( var message in permanentMessages )
			{
				if ( pair.Key == message )
				{
					wasFound = true;

					break;
				}
			}

			if ( !wasFound )
				messagesToRemove.Add( pair.Key );
		}

		foreach ( var message in messagesToRemove )
		{
			eventTable.Remove( message );
		}
	}

	public static void PrintEventTable()
	{
		Debugger.Log( LogChannelName, $"PrintEventTable: " );
		Debugger.Log( "\t\t\t=== Table ===" );

		foreach ( var pair in eventTable )
		{
			Debugger.Log( $"\t\t\t{pair.Key}\t\t{pair.Value}" );
		}

		Debugger.Log( "\n" );
	}

	#endregion Helper methods

	#region Message logging and exception throwing

	public static void OnListenerAdding( string eventType, Delegate listenerBeingAdded )
	{
		#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
		Debugger.Log( LogChannelName,
			$"OnListenerAdding() -- '{eventType}'\t[{listenerBeingAdded.Target} -> {listenerBeingAdded.Method}]" );
		#endif

		if ( !eventTable.ContainsKey( eventType ) )
		{
			eventTable.Add( eventType, null );
		}

		var d = eventTable[ eventType ];

		if ( d != null && d.GetType() != listenerBeingAdded.GetType() )
		{
			throw new ListenerException(
				$"Attempting to add listener with inconsistent signature for event type {eventType}. "
				+ $"Current listeners have type {d.GetType().Name} and listener being added has type {listenerBeingAdded.GetType().Name}" );
		}
	}

	public static void OnListenerRemoving( string eventType, Delegate listenerBeingRemoved )
	{
		#if LOG_ALL_MESSAGES
		Debugger.Log( LogChannelName,
			$" OnListenerRemoving() --  \t\"{eventType}\"\t{{{listenerBeingRemoved.Target} -> {listenerBeingRemoved.Method}}}" );
		#endif

		if ( eventTable.ContainsKey( eventType ) )
		{
			var d = eventTable[ eventType ];

			if ( d == null )
			{
				throw new ListenerException(
					$"Attempting to remove listener with for event type \"{eventType}\" but current listener is null." );
			}
			else if ( d.GetType() != listenerBeingRemoved.GetType() )
			{
				throw new ListenerException(
					$"Attempting to remove listener with inconsistent signature for event type {eventType}. "
					+ $"Current listeners have type {d.GetType().Name} and listener being removed has type {listenerBeingRemoved.GetType().Name}" );
			}
		}
		else
		{
			throw new ListenerException(
				$"Attempting to remove listener for type \"{eventType}\" but Messenger doesn't know about this event type." );
		}
	}

	public static void OnListenerRemoved( string eventType )
	{
		if ( eventTable[ eventType ] == null )
		{
			eventTable.Remove( eventType );
		}
	}

	public static void OnBroadcasting( string eventType )
	{
		#if REQUIRE_LISTENER
		if ( !eventTable.ContainsKey( eventType ) )
		{
			throw new BroadcastException(
				$"Broadcasting message \"{eventType}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent." );
		}
		#endif
	}

	public static BroadcastException CreateBroadcastSignatureException( string eventType )
	{
		return new BroadcastException(
			$"Broadcasting message \"{eventType}\" but listeners have a different signature than the broadcaster." );
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
	public static void AddListener( string eventType, Callback handler )
	{
		OnListenerAdding( eventType, handler );
		eventTable[ eventType ] = (Callback) eventTable[ eventType ] + handler;
	}

	//Single parameter
	public static void AddListener<T>( string eventType, Callback<T> handler )
	{
		OnListenerAdding( eventType, handler );
		eventTable[ eventType ] = (Callback<T>) eventTable[ eventType ] + handler;
	}

	//Two parameters
	public static void AddListener<T1, T2>( string eventType, Callback<T1, T2> handler )
	{
		OnListenerAdding( eventType, handler );
		eventTable[ eventType ] = (Callback<T1, T2>) eventTable[ eventType ] + handler;
	}

	//Three parameters
	public static void AddListener<T1, T2, T3>( string eventType, Callback<T1, T2, T3> handler )
	{
		OnListenerAdding( eventType, handler );
		eventTable[ eventType ] = (Callback<T1, T2, T3>) eventTable[ eventType ] + handler;
	}

	#endregion AddListener

	#region RemoveListener

	//No parameters
	public static void RemoveListener( string eventType, Callback handler )
	{
		OnListenerRemoving( eventType, handler );
		eventTable[ eventType ] = (Callback) eventTable[ eventType ] - handler;
		OnListenerRemoved( eventType );
	}

	//Single parameter
	public static void RemoveListener<T>( string eventType, Callback<T> handler )
	{
		OnListenerRemoving( eventType, handler );
		eventTable[ eventType ] = (Callback<T>) eventTable[ eventType ] - handler;
		OnListenerRemoved( eventType );
	}

	//Two parameters
	public static void RemoveListener<T1, T2>( string eventType, Callback<T1, T2> handler )
	{
		OnListenerRemoving( eventType, handler );
		eventTable[ eventType ] = (Callback<T1, T2>) eventTable[ eventType ] - handler;
		OnListenerRemoved( eventType );
	}

	//Three parameters
	public static void RemoveListener<T1, T2, T3>( string eventType, Callback<T1, T2, T3> handler )
	{
		OnListenerRemoving( eventType, handler );
		eventTable[ eventType ] = (Callback<T1, T2, T3>) eventTable[ eventType ] - handler;
		OnListenerRemoved( eventType );
	}

	#endregion RemoveListener

	#region Broadcast

	//No parameters
	public static void Broadcast( string eventType )
	{
		#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debugger.Log( LogChannelName, $"Broadcast\t\t\tInvoking \t\"{eventType}\"" );
		#endif
		OnBroadcasting( eventType );

		if ( eventTable.TryGetValue( eventType, out var d ) )
		{
			if ( d is Callback callback )
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
	public static void Broadcast<T>( string eventType, T arg1 )
	{
		#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debugger.Log( LogChannelName, $"Broadcast\t\t\tInvoking \t\"{eventType}\"" );
		#endif
		OnBroadcasting( eventType );

		if ( eventTable.TryGetValue( eventType, out var d ) )
		{
			if ( d is Callback<T> callback )
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
	public static void Broadcast<T1, T2>( string eventType, T1 arg1, T2 arg2 )
	{
		#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debugger.Log( LogChannelName, $"Broadcast\t\t\tInvoking \t\"{eventType}\"" );
		#endif
		OnBroadcasting( eventType );

		if ( eventTable.TryGetValue( eventType, out var d ) )
		{
			if ( d is Callback<T1, T2> callback )
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
	public static void Broadcast<T1, T2, T3>( string eventType, T1 arg1, T2 arg2, T3 arg3 )
	{
		#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debugger.Log( LogChannelName, $"Broadcast\t\t\tInvoking \t\"{eventType}\"" );
		#endif
		OnBroadcasting( eventType );

		if ( eventTable.TryGetValue( eventType, out var d ) )
		{
			if ( d is Callback<T1, T2, T3> callback )
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