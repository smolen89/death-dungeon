using System;
using RD.ECS.Entities;

namespace RD.ECS.Message
{
	public readonly struct EntityDisposingMessage : IMessage
	{
		public readonly IWorld World;
		public readonly IEntity Entity;

		public EntityDisposingMessage( IWorld world, IEntity entity )
		{
			this.World = world;
			this.Entity = entity;
		}
	}
}