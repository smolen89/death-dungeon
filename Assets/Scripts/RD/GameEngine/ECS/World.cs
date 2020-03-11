using System;
using System.Collections.Generic;
using RD.GameEngine.ECS.Components;
using RD.GameEngine.ECS.Entities;
using RD.Util;

namespace RD.GameEngine.ECS
{
	public sealed class World
	{
		private readonly ListEx<Entity> deleted;
		private readonly Dictionary<string, IEntityTemplate> entityTemplates;
		private readonly Dictionary<Type, IComponentPool<ComponentPoolable>> pools;
		private readonly HashSet<Entity> refreshed;
		private int poolCleanupDelayCounter;
		private bool isInitialized = false;

		public World( bool isSortedEntities = false, bool processAttributes = true, bool initializeAll = false )
		{
			refreshed = new HashSet<Entity>();
			pools = new Dictionary<Type, IComponentPool<ComponentPoolable>>();
			entityTemplates = new Dictionary<string, IEntityTemplate>();
			deleted = new ListEx<Entity>(  );
		}
	}

	public class Entity
	{
	}

	public interface IEntityTemplate
	{
		
	}

	public sealed class EntityManager
	{
		private ListEx<ListEx<IComponent>> componentsByType;
		private readonly ListEx<Entity> removedAndAvailableEntities;
		private readonly HashSet<Tuple<Entity, ComponentType>> componentsToBeRemoved = new HashSet<Tuple<Entity, ComponentType>>();
		
	}
}