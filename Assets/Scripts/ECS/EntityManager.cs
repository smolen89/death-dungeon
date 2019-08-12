using System.Collections.Generic;
using RD.ECS.Components;
using RD.ECS.Entities;
using RD.ECS.Systems;

namespace RD.ECS
{
	public sealed class EntityManager
	{
		// Lista wszystkich entity
		// Oznaczenie Entity Parenów i childów
		private IList<IEntity> entities;

		private IEntity CreateEntity;

		private IEntity GetEntity( int entityID ) => entities[ entityID ];

		private void RemoveEntity( int entityID ) => entities.RemoveAt( entityID );
	}

	public interface IEntityManager
	{
		IList<IEntity> entities { get; }

		IEntity CreateEntity();

		IEntity GetEntity( int entityID );

		void RemoveEntity( int entityID );

		void SetParent( int entityID, int parentEntityID );

		void SetChild( int entityID, int childEntityID );

		IEntity[] GetChilds( int entityID );

		IEntity GetParent( int entityID );
	}

	public struct EntityEntry
	{
		public int parentEntityID;
		public List<int> childEntityID;
		public List<IComponent> components;
	}
}