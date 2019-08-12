using RD.ECS.Components;
using RD.ECS.Events_Tiny;
using System;
using System.Collections.Generic;

namespace RD.ECS.Entities
{
	public interface IEntityManager
	{
		IEntity CreateEntity( string name = null );

		bool DestroyEntity( uint entityID );

		void AddComponent<TComponent>( uint entityID, TComponent component = default ) where TComponent : struct, IComponent;

		void RemoveComponent<TComponent>( uint entityID ) where TComponent : struct, IComponent;

		void RemoveAllComponents( uint entityID );

		bool HasComponent<TComponent>( uint entityID ) where TComponent : struct, IComponent;

		bool HasComponent( uint entityID, Type componentType );

		IEntity GetEntity( uint entityID );

		List<uint> GetEntitiesWithAll( params Type[] components );

		List<uint> GetEntitiesWithAny( params Type[] components );

		IComponentIterator GetComponentIterator( uint entityID );

		IEventManager EventManager { get; }

		uint NumOfActiveEntities { get; }

		uint NumOfReusableEntities { get; }

		uint NumOfActiveComponents { get; }

		uint AverageNumOfComponentsPerEntity { get; }
	}
}