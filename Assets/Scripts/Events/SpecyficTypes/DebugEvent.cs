// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi


using RD.GameEngine.Events.Sandbox;

namespace RD.GameEngine.Events
{
	public class DebugEvent : GameEvent<DebugEvent>
	{
		public int VerbosityLevel;
	}

}