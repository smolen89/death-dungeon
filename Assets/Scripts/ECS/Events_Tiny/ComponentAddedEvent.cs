using System;

namespace RD.ECS.Events_Tiny
{
	public struct ComponentAddedEvent : IEvent
	{
		/// <summary>
		/// Czy chodzi o Entity?? czy i system w którym został dodany komponent do entity?
		/// </summary>
		public uint EntityID;

		public Type ComponentType;

		public ComponentAddedEvent( uint entityID, Type componentType )
		{
			this.EntityID = entityID;
			this.ComponentType = componentType ?? throw new ArgumentNullException( nameof( componentType ) );
		}
	}
}