using System.Collections.Generic;
using RD.ECS.Components;
using RD.ECS.Entities;
using RD.ECS.Events_Tiny;

namespace RD.ECS.Contexts
{
	/// <summary>
	/// The interface describes a functionality of a world's context which is a main hub
	/// that combines all parts of the architecture.
	/// </summary>
	public interface IContext
	{
		/// <summary>
		/// Method creates a new Entity in Context.
		/// </summary>
		/// <param name="blueprintName">Name blueprint to load entity from EntityFactory.</param>
		/// <returns>EntityID of created entity.</returns>
		uint CreateEntity( string blueprintName = null );

		bool DestroyEntity( uint entityID );

		IEntity GetEntityByID( uint entityID );

		List<IEntity> GetEntitiesWithAll( params IComponent[] components );

		List<IEntity> GetEntitiesWithAny( params IComponent[] components );

		IEventManager EventManager { get; }
		IEntityManager EntityManager { get; }
		IComponentManager ComponentManager { get; }

		ContextStats Statistics { get; }
	}
}