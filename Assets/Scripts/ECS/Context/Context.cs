using System;
using System.Collections.Generic;
using RD.ECS.Components;
using RD.ECS.Entities;
using RD.ECS.Events_Tiny;

namespace RD.ECS.Contexts
{
	public class Context : IContext
	{
		public Context()
		{
		}

		protected readonly HashSet<IEntity> entities = new HashSet<IEntity>();
		protected int entitiesCount;

		protected readonly Stack<IEntity> reusableEntities = new Stack<IEntity>();
		protected int reusableEntititesCount;

		protected delegate bool FilterPredicate( uint entityId, params Type[] componentTypes );

		protected Queue<uint> nextFreeEntityId;
		protected uint entitiesIdCounter;

		protected static string defaultEntityPatternName = "Entity {0}";

		public uint CreateEntity( string name = null )
		{
			uint entityID = 0;
			entityID = nextFreeEntityId.Count > 0 ? nextFreeEntityId.Dequeue() : entitiesIdCounter++;

			IEntity entity = null;
			if( reusableEntities.Count > 0 )
			{
				entity = reusableEntities.Pop();
			}
			else
			{
				///entity = new Entity( entityID, name ?? string.Format( defaultEntityPatternName, entityID ) );
			}

			entities.Add( entity );
			return entityID;
		}

		public bool DestroyEntity( uint entityID ) => throw new NotImplementedException();

		public IEntity GetEntityByID( uint entityID ) => throw new NotImplementedException();

		public List<IEntity> GetEntitiesWithAll( params IComponent[] components ) => throw new NotImplementedException();

		public List<IEntity> GetEntitiesWithAny( params IComponent[] components ) => throw new NotImplementedException();

		public IEventManager EventManager { get; }
		public IEntityManager EntityManager { get; }
		public IComponentManager ComponentManager { get; }
		public ContextStats Statistics { get; }

		public override string ToString()
		{
			return $"Entities: {entitiesCount}, \n ReusableEntities: {reusableEntititesCount}";
		}
	}
}