using System;
using System.Collections.Generic;
using RD.GameEngine.ECS.Components;
using RD.GameEngine.ECS.Events;
using RD.Util;

namespace RD.GameEngine.ECS.Entities
{
	public interface IEntity : IDisposable, IPrettyPrintable
	{
		int UniqueId { get; }
		string Id { get; }
		string Description { get; }

		List<string> ChildrenEntities { get; }
		string ParentEntity { get; }
		
		IComponent GetComponent<TComponent>() where TComponent : IComponent;
		bool HasComponent<TComponent>() where TComponent : IComponent;
		void RemoveComponent<TComponent>() where TComponent : IComponent;
		int ComponentCount { get; }
		
		List<TFlag> Flags<TFlag>() where TFlag : Type;
		bool HasFlag<TFlag>() where TFlag : Type;

		void PropagateEvent( IGameEvent gameEvent );
		bool WillRespondTo( IGameEvent gameEvent );

		bool IsValid { get; }
		bool IsEnabled { get; }
	}
}