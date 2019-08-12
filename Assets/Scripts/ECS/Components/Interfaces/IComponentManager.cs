using RD.ECS.Events_Tiny;

namespace RD.ECS.Components
{
	/// <summary>
	/// Interfejs opisujący funkcjonalność ComponentManagera jaką powienien mieć.
	/// </summary>
	public interface IComponentManager
	{
		//todo add comments
		void RegisterEntity( uint entityID );

		void AddComponent<TComponent>( uint entityID, TComponent componentInitializer = default ) where TComponent : struct, IComponent;

		TComponent GetComponent<TComponent>( uint entityID ) where TComponent : struct, IComponent;

		void RemoveComponent<TComponent>( uint entityID ) where TComponent : struct, IComponent;

		void RemoveAllComponents( uint entityID );

		bool HasComponent<TComponent>( uint entityID ) where TComponent : struct, IComponent;

		IComponentIterator GetComponentIterator( uint entityID );

		IEventManager EventManager { get; }

		uint NumOfActiveComponents { get; }
		uint NumOfAverageComponentsPerEntity { get; }
	}
}