using RD.ECS.Components;
using RD.ECS.Entities;

namespace RD.ECS.Message
{
	public readonly struct ComponentRemovedMessage : IMessage
	{
		public readonly IEntity Entity;
		public readonly IComponent Component;

		public ComponentRemovedMessage( IEntity entity, IComponent component )
		{
			this.Entity = entity;
			this.Component = component;
		}
	}
}