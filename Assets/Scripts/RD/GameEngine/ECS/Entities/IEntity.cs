using System;

namespace RD.GameEngine.ECS.Entities
{
	public interface IEntity
	{
		string Id { get; }
		int UniqueId { get; }
		string Name { get; }
		string Description { get; }
		bool IsEnabled { get; }
		bool HasComponent<T>();
		bool HasComponent( Type componentType );
		void AddComponent<T>();
		void RemoveCompoenent<T>();
		void RemoveAllComponents();
	}
}