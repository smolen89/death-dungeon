// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RDE.Engine.Systems
{
	public abstract class GameEvent<T> where T: GameEvent<T>
	{
		private bool hasFired;
		private static event EventListener listeners;

		public string Description;
		public delegate void EventListener( T info );
		
		public static void RegisterListener(EventListener listener )
		{
			listeners += listener;
		}

		public static void UnregisterListener(EventListener listener )
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

	public class DebugEvent : GameEvent<DebugEvent>
	{
		public int VerbosityLevel;
	}

	public class UnitDeathEvent : GameEvent<UnitDeathEvent>
	{
		public GameObject UnitGO;
		// Info about cause of death, our killer, etc..
		// Could be a struct, readonly, etc..
	}

}