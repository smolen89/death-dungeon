using System;
using RD.GameEngine.ECS.Components;
using RD.Util;

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
		void AddComponent( IComponent component );
		void RemoveCompoenent<T>();
		void RemoveAllComponents();

		void Delete();
		
		ListEx<IComponent> Components { get; }
		bool RefreshingState { get; set; }
	}
}