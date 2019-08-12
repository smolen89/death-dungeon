using RD.ECS.Components;
using RD.ECS.Entities;

namespace RD.ECS.Message
{
	public readonly struct ComponentReplaceMessage : IMessage
	{
		public readonly IEntity Entity;
		public readonly IComponent OldComponent;
		public readonly IComponent NewComponent;

		public ComponentReplaceMessage( IEntity entity, IComponent oldComponent, IComponent newComponent )
		{
			this.Entity = entity;
			this.OldComponent = oldComponent;
			this.NewComponent = newComponent;
		}
	}

}