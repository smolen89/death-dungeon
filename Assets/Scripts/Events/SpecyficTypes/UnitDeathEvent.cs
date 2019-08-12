// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi

using RD.GameEngine.Events.Sandbox;
using UnityEngine;

namespace RD.GameEngine.Events
{
	public class UnitDeathEvent : GameEvent<UnitDeathEvent>
	{
		public UnitDeathEvent( )
		{

		}
		public GameObject UnitGO;
		// Info about cause of death, our killer, etc..
		// Could be a struct, readonly, etc..
	}

	public class CheckMovementDirectionModificationGameEvent : GameEvent<CheckMovementDirectionModificationGameEvent>,IGameEvent
	{
		public CheckMovementDirectionModificationGameEvent(Direction direction )
		{
			this.direction = direction;
		}
		public Direction direction;
	}

}