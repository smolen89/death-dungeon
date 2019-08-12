// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RD.GameEngine.Events.Sandbox
{
	public abstract class GameEvent<T>  where T : GameEvent<T> 
	{
		private bool hasFired;
		private static event EventListener listeners;

		public string Description;
		public delegate void EventListener( T info );

		public static void RegisterListener( EventListener listener )
		{
			listeners += listener;
		}

		public static void UnregisterListener( EventListener listener )
		{
			listeners -= listener;
		}

		public void FireEvent()
		{
			if( hasFired )
			{
				Debugger.Log( "GameEvent", $"This event has already fired, to prevent infinite loops you can't refire an event." );
				return;
			}

			hasFired = true;

			listeners?.Invoke( this as T );
		}
	}
}