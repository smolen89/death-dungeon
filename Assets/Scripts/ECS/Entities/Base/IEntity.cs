using System;
using System.Collections.Generic;
using RD.ECS.Components;
using RD.ECS.Message;

namespace RD.ECS.Entities
{
	public interface IEntity : IDisposable, IEquatable<IEntity>
	{
		void Initialize( int entityID );

		void Reactivate( int newID );

		int ID { get; }

		/// <summary>
		/// Get cashed IsEnabledComponent from this enabled
		/// </summary>
		bool IsEnabled { get; }

		Func<int, bool> ParentEntity { get; set; }
		HashSet<int> ChildEntityID { get; set; }

		int totalComponents { get; }

		void AddComponent<TComponent>( in TComponent componentData = default ) where TComponent : IComponent;

		void RemoveComponent<TComponent>() where TComponent : IComponent;

		void RemoveAllComponents();

		TComponent GetComponent<TComponent>() where TComponent : IComponent;

		IComponent[] GetComponents();

		bool HasComponent<TComponent>() where TComponent : IComponent;

		bool HasComponent( Type componentType );
	}
}