using RD.ECS.Components;
using RD.ECS.Entities;

namespace RD.ECS.Message
{
	public readonly struct ComponentAddedMessage : IMessage
	{
		public readonly IEntity Entity;
		public readonly IComponent Component;

		public ComponentAddedMessage( IEntity entity, IComponent component )
		{
			this.Entity = entity;
			this.Component = component;
		}
	}
}