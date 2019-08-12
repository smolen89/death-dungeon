using RD.ECS.Entities;

namespace RD.ECS.Message
{
	public readonly struct EntityDisposedMessage : IMessage
	{
		public readonly IWorld World;
		public readonly IEntity Entity;

		public EntityDisposedMessage( IWorld world, IEntity entity )
		{
			this.World = world;
			this.Entity = entity;
		}
	}
}