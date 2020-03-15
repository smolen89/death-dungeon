using System;
using System.Numerics;
using RD.GameEngine.ECS.Components;
using RD.Util;

namespace RD.GameEngine.ECS.Entities
{
	public interface IEntity
	{
		int Id { get; }
		long UniqueId { get; }
		BigInteger TypeBits { get; set; }
		BigInteger SystemBits { get; set; }

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
		bool DeletingState { get; set; }
		void AddSystemBit( BigInteger bit );
		void RemoveSystemBit( BigInteger bit );
	}
}