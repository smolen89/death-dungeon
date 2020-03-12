using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RD.Exceptions;
using RD.GameEngine.ECS.Components;
using RD.GameEngine.ECS.Entities;
using RD.Util;
using UnityEngine;

namespace RD.GameEngine.ECS
{
	public sealed class World
	{
		private readonly ListEx<IEntity> deleted;
		private readonly Dictionary<string, IEntityTemplate> entityTemplates;
		private readonly Dictionary<Type, IComponentPool<ComponentPoolable>> pools;
		private readonly HashSet<IEntity> refreshed;
		private int poolCleanupDelayCounter;
		private bool isInitialized = false;

		public World( bool isSortedEntities = false, bool processAttributes = true, bool initializeAll = false )
		{
			refreshed = new HashSet<IEntity>();
			pools = new Dictionary<Type, IComponentPool<ComponentPoolable>>();
			entityTemplates = new Dictionary<string, IEntityTemplate>();
			deleted = new ListEx<IEntity>();
		}

		public Dictionary<IEntity, ListEx<IComponent>> CurrentState
		{
			get
			{
				ListEx<IEntity> entities = EntityManager.ActiveEntities;
				Dictionary<IEntity, ListEx<IComponent>> currentState = new Dictionary<IEntity, ListEx<IComponent>>();

				for ( int index = 0; index < entities.Count; index++ )
				{
					IEntity entity = entities.Get( index );

					if ( entity != null )
					{
						ListEx<IComponent> components = entity.Components;
						currentState.Add( entity, components );
					}
				}

				return currentState;
			}
		}

		public float DeltaTime { get; private set; }

		public IEntityManager EntityManager { get; private set; }
		public IGroupManager GroupManager { get; private set; }
		public int PoolCleanupDelay { get; set; }
		public ISystemManager SystemManager { get; private set; }
		public ITagManager TagManager { get; private set; }
		internal bool IsSortedEntities { get; private set; }

		public void Clear()
		{
			foreach ( IEntity activeEntity in EntityManager.ActiveEntities.Where( activeEntity => activeEntity != null ) )
			{
				activeEntity.Delete();
			}

			Update();
		}

		public IEntity CreateEntiity( long? entityUniqueId = null ) => EntityManager.Create( entityUniqueId );

		public IEntity CreateEntityFromTemplate( string entityTemplateTag, params object[] templateArgs )
		{
			return CreateEntityFromTemplate( null, entityTemplateTag, templateArgs );
		}

		public IEntity CreateEntityFromTemplate( long entityUniqueId, string entityTemplateTag, params object[] templateArgs )
		{
			return CreateEntityFromTemplate( (long?) entityUniqueId, entityTemplateTag, templateArgs );
		}

		public IEntity LoadEntityState( string templateTag, string groupName, IEnumerable<IComponent> components, params object[] templateArgs )
		{
			Debug.Assert( components != null, $"Components must not be null." );

			IEntity entity;

			if ( !string.IsNullOrEmpty( templateTag ) )
			{
				entity = CreateEntityFromTemplate( templateTag, -1, templateArgs );
			}
			else
			{
				entity = CreateEntiity();
			}

			if ( !string.IsNullOrEmpty( groupName ) )
			{
				GroupManager.Set( groupName, entity );
			}

			foreach ( IComponent component in components )
			{
				entity.AddComponent(component);
			}

			return entity;
		}
		
		public void SetEntityTemplate( string entityTag, IEntityTemplate entityTemplate )
		{
			entityTemplates.Add( entityTag, entityTemplate );
		}
		public void SetPool( Type type, IComponentPool<ComponentPoolable> pool )
		{
			Debug.Assert( type != null,$"Type must not be null." );
			Debug.Assert( pool !=null,$"ComponentPool must not be null." );

			pools.Add( type, pool );
		}
		
		public void Update() => Update(Time.deltaTime);

		public void Update( float deltaTime )
		{
			DeltaTime = deltaTime;
			EntityManager.RemoveMarkedComponents();
			poolCleanupDelayCounter++;

			if ( poolCleanupDelayCounter > PoolCleanupDelay )
			{
				poolCleanupDelayCounter = 0;

				foreach ( Type poolsKey in pools.Keys )
				{
					pools[ poolsKey ].CleanUp();
				}
			}

			if ( !deleted.IsEmpty )
			{
				for ( int i = deleted.Count; i >= 0; i-- )
				{
					IEntity entity = deleted.Get( i );
					TagManager.Unregister( entity );
					GroupManager.Remove( entity );
					EntityManager.Remove( entity );
					DeletingState = false;
				}
				deleted.Clear();
			}

			bool isRefreshing = refreshed.Count > 0;

			if ( isRefreshing )
			{
				foreach ( IEntity entity in refreshed )
				{
					EntityManager.Refresh( entity );
					entity.RefreshingState = false;
				}
				refreshed.Clear();
			}

			SystemManager.Update();
		}

		public void Draw() => SystemManager.Draw();

		public void UnloadContent() => SystemManager.TerminateAll();

		internal void RefreshEntity( IEntity entity )
		{
			Debug.Assert( entity != null, $"Entity must not be null." );

			this.refreshed.Add( entity );
		}

		private IEntity CreateEntityFromTemplate( long? entityUniqueId, string entityTemplateTag, params object[] templateArgs )
		{
			Debug.Assert( !string.IsNullOrEmpty( entityTemplateTag ), $"Entity template tag must not be null or empty." );

			IEntity entity = EntityManager.Create( entityUniqueId );
			IEntityTemplate entityTemplate;
			entityTemplates.TryGetValue( entityTemplateTag, out entityTemplate );

			if ( entityTemplate == null )
			{
				throw new MissingEntityTemplateException( entityTemplateTag );
			}

			entity = entityTemplate.BuildEntity( entity, this, templateArgs );
			RefreshEntity( entity );

			return entity;
		}
	}

	public class Entity
	{
	}

	public interface IEntityTemplate
	{
		void TryGetValue( string entityTemplateTag, out IEntityTemplate entityTemplate );
		IEntity BuildEntity( IEntity entity, World world, object[] templateArgs );
	}

	public interface IEntityManager
	{
		ListEx<IEntity> ActiveEntities { get; }
		IEntity Create( long? entityUniqueId );
		void RemoveMarkedComponents();
		void Remove( IEntity entity );
		void Refresh( IEntity entity );
	}

	public sealed class EntityManager
	{
		private ListEx<ListEx<IComponent>> componentsByType;
		private readonly ListEx<Entity> removedAndAvailableEntities;
		private readonly HashSet<Tuple<Entity, ComponentType>> componentsToBeRemoved = new HashSet<Tuple<Entity, ComponentType>>();
	}

	public interface IGroupManager
	{
		void Remove( IEntity entity );
		void Set( string groupName, IEntity entity );
	}

	public interface ISystemManager
	{
		void TerminateAll();
		void Draw();
		void Update();
	}

	public interface ITagManager
	{
		void Unregister( IEntity entity );
	}
}