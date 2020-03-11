using System;
using RD.Util;

namespace RD.GameEngine.ECS.Entities
{
	public interface IEntitySet : IDisposable
	{
		event ActionIn<IEntity> EntityAdded;
		event ActionIn<IEntity> EntityRemoved;
		int Count { get; }
		void Complete();

		IEntity[] GetEntities();
		IEntity[] GetEntities( int start, int length );
	}
}