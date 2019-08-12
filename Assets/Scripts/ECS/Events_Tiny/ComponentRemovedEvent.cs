using System;

namespace RD.ECS.Events_Tiny
{
	public struct ComponentRemovedEvent : IEvent
	{
		public uint EntityID;
		public Type ComponentType;

		public ComponentRemovedEvent( uint entityID, Type componentType )
		{
			this.EntityID = entityID;
			this.ComponentType = componentType ?? throw new ArgumentNullException( nameof( componentType ) );
		}
	}
}