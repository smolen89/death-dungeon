using RD.ECS.Entities;

namespace RD.ECS.Message
{
	public readonly struct EntityCreatedMessage : IMessage
	{
		public readonly IWorld World;
		public readonly IEntity Entity;

		public EntityCreatedMessage( IWorld world, IEntity entity )
		{
			this.World = world;
			this.Entity = entity;
		}
	}
}