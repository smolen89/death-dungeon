// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System;
using System.Collections.Generic;
using RD.ECS.Events_Tiny;

namespace RD.ECS.Components
{
	public partial class ComponentManager : IComponentManager
	{
		protected IEventManager eventManager;
		protected IDictionary<uint, IDictionary<Type, int>> entity2ComponentsHashTable; // Dict<entityID,Dict<ComponentType,indexIn>>??
		protected IDictionary<Type, int> componentTypesHashTable;
		protected IList<IList<IComponent>> componentsMatrix;    // first index is a component type hash, second is entity ID

		protected uint numOfActiveComponents;

		public ComponentManager( IEventManager eventManager )
		{
			this.eventManager = eventManager ?? throw new ArgumentNullException( nameof( eventManager ) );
			entity2ComponentsHashTable = new Dictionary<uint, IDictionary<Type, int>>();
			componentsMatrix = new List<IList<IComponent>>();
			componentTypesHashTable = new Dictionary<Type, int>();
			numOfActiveComponents = 0;
		}

		public void RegisterEntity( uint entityID )
		{
			// cza sprawdzić czy dany Entity jest już w bazie.
			if( entity2ComponentsHashTable.ContainsKey( entityID ) )
			{
				return;
			}
			entity2ComponentsHashTable.Add( entityID, new Dictionary<Type, int>() );
		}

		public void AddComponent<TComponent>( uint entityID, TComponent componentInitializer = default ) where TComponent : struct, IComponent
		{
			RegisterEntity( entityID );
			var entityComponentTable = entity2ComponentsHashTable[ entityID ];
			var cachedComponentType = typeof( TComponent );

			if( !componentTypesHashTable.ContainsKey( cachedComponentType ) )
			{
				componentTypesHashTable.Add( cachedComponentType, componentsMatrix.Count );
				componentsMatrix.Add( new List<IComponent>() );
			}

			int componentGroupHashValue = componentTypesHashTable[ cachedComponentType ];
			var componentGroup = componentsMatrix[ componentGroupHashValue ];

			if( entityComponentTable.ContainsKey( cachedComponentType ) )
			{
				componentGroup[ entityComponentTable[ cachedComponentType ] ] = componentInitializer;
				NotifyOnComponentsChanged( entityID, componentInitializer );
				return;
			}

			// create a new Component
			entityComponentTable.Add( cachedComponentType, componentGroup.Count );
			componentGroup.Add( componentInitializer );

			NotifyOnComponentsChanged( entityID, componentInitializer );

			numOfActiveComponents++;
		}

		public TComponent GetComponent<TComponent>( uint entityID ) where TComponent : struct, IComponent
		{
			if( !entity2ComponentsHashTable.ContainsKey( entityID ) )
			{
				throw new EntityDoesntExistException( entityID );
			}

			var entityComponentTable = entity2ComponentsHashTable[ entityID ];

			Type cachedComponentType = typeof( TComponent );

			if( !entityComponentTable.ContainsKey( cachedComponentType ) )
			{
				throw new ComponentDoesntExistException( cachedComponentType, entityID );
			}

			int componentTypeGroupHashValue = componentTypesHashTable[ cachedComponentType ];
			int componentHashValue = entityComponentTable[ cachedComponentType ];

			return (TComponent)componentsMatrix[ componentTypeGroupHashValue ][ componentHashValue ];
		}

		public void RemoveComponent<TComponent>( uint entityID ) where TComponent : struct, IComponent
		{
			if( !entity2ComponentsHashTable.ContainsKey( entityID ) )
			{
				throw new EntityDoesntExistException( entityID );
			}

			var entityComponentsTable = entity2ComponentsHashTable[ entityID ];
			RemoveComponent( entityComponentsTable, typeof( TComponent ), entityID );

			ComponentRemovedEvent componentRemoved
				= new ComponentRemovedEvent( entityID, typeof( TComponent ) );

			eventManager.Notify( componentRemoved );
		}

		public void RemoveAllComponents( uint entityID )
		{
			if( !entity2ComponentsHashTable.ContainsKey( entityID ) )
			{
				throw new EntityDoesntExistException( entityID );
			}

			IDictionary<Type, int> entityComponentsTable = entity2ComponentsHashTable[ entityID ];

			while( entityComponentsTable.Count > 0 )
			{
				RemoveComponent( entityComponentsTable, entityComponentsTable.Keys.GetEnumerator().Current, entityID );
			}
		}

		public bool HasComponent<TComponent>( uint entityID ) where TComponent : struct, IComponent
		{
			return HasComponent( entityID, typeof( TComponent ) );
		}

		public bool HasComponent( uint entityID, Type componentType )
		{
			if( !entity2ComponentsHashTable.ContainsKey( entityID ) )
			{
				throw new EntityDoesntExistException( entityID );
			}
			var entityComponentsTable = entity2ComponentsHashTable[ entityID ];
			return entityComponentsTable.ContainsKey( componentType );
		}

		public IComponentIterator GetComponentIterator( uint entityID )
		{
			if( !entity2ComponentsHashTable.ContainsKey( entityID ) )
			{
				throw new EntityDoesntExistException( entityID );
			}

			return new ComponentIterator( entity2ComponentsHashTable[ entityID ], componentTypesHashTable, componentsMatrix );
		}

		public IEventManager EventManager => eventManager;

		public uint NumOfActiveComponents => numOfActiveComponents;

		public uint NumOfAverageComponentsPerEntity
		{
			get
			{
				uint numOfEntities = (uint)entity2ComponentsHashTable.Count;
				uint avgNumOfComponentsPerEntity = 0;

				if( numOfEntities < 1 ) return 0;

				var entitiesIter = entity2ComponentsHashTable.Keys.GetEnumerator();

				while( entitiesIter.MoveNext() )
				{
					avgNumOfComponentsPerEntity += (uint)entity2ComponentsHashTable[ entitiesIter.Current ].Count;
				}
				return avgNumOfComponentsPerEntity / numOfEntities;
			}
		}

		private void RemoveComponent( IDictionary<Type, int> entityComponentsTable, Type componentType, uint entityID )
		{
			if( !entityComponentsTable.ContainsKey( componentType ) )
			{
				throw new ComponentDoesntExistException( componentType, entityID );
			}
			entityComponentsTable.Remove( componentType );
			numOfActiveComponents--;
		}

		private void NotifyOnComponentsAdded<TComponent>( uint entityID, TComponent newComponent )
		{
			//bug prawdopodobnie powinno być generic type!!
			ComponentAddedEvent componentAdded
				= new ComponentAddedEvent( entityID, typeof( TComponent ) );

			eventManager.Notify( componentAdded );
		}

		private void NotifyOnComponentsChanged<TComponent>( uint entityID, TComponent value )
		{
			ComponentChangedEvent<TComponent> componentChanged
				= new ComponentChangedEvent<TComponent>( entityID, value );

			ComponentAddedEvent componentAdded
				= new ComponentAddedEvent( entityID, typeof( TComponent ) );

			eventManager.Notify( componentAdded );
			eventManager.Notify( componentChanged );
		}
	}
}