using System;

namespace RD.ECS.Events_Tiny
{
	public struct ListenerEntry
	{
		public Type EventType;
		public IEventListener Listener;
	}
}