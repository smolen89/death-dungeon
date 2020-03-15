using System;
using System.Collections.Generic;
using RD.GameEngine.ECS.Components;
using RD.GameEngine.ECS.Entities;
using RD.Util;

namespace RD.GameEngine.ECS.Managers
{
	public sealed class EntityManager
	{
		private ListEx<ListEx<IComponent>> componentsByType;
		private readonly ListEx<Entity> removedAndAvailableEntities;
		private readonly HashSet<Tuple<Entity, ComponentType>> componentsToBeRemoved = new HashSet<Tuple<Entity, ComponentType>>();

		public EntityManager( World world )
		{
		}

		public ListEx<IEntity> ActiveEntities { get; set; }

		public IEntity Create( long? entityUniqueId )
		{
			return null;
		}

		public IEntity GetEntity( int entityId )
		{
			return null;
		}

		public void RemoveMarkedComponents()
		{
		}

		public void Remove( IEntity entity )
		{
		}

		public void Refresh( IEntity entity )
		{
		}
	}
}