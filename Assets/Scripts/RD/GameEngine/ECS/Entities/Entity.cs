using System;
using System.Collections.Generic;
using RD.GameEngine.ECS.Components;
using RD.GameEngine.ECS.Events;

namespace RD.GameEngine.ECS.Entities
{
	public class Entity:IEntity
	{
		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
		}

		public string PrettyPrintable( int indentLevel )
		{
			return null;
		}

		public int UniqueId { get; }
		public string Id { get; }
		public string Description { get; }
		public List<string> ChildrenEntities { get; }
		public string ParentEntity { get; }
		public int ComponentCount { get; }
		public bool IsValid { get; }
		public bool IsEnabled { get; }
		
		public IComponent GetComponent<TComponent>() where TComponent : IComponent
		{
			return null;
		}

		public bool HasComponent<TComponent>() where TComponent : IComponent
		{
			return false;
		}

		public void RemoveComponent<TComponent>() where TComponent : IComponent
		{
		}


		
		public List<TFlag> Flags<TFlag>() where TFlag : Type
		{
			return null;
		}

		public bool HasFlag<TFlag>() where TFlag : Type
		{
			return false;
		}

		public void PropagateEvent( IGameEvent gameEvent )
		{
		}

		public bool WillRespondTo( IGameEvent gameEvent )
		{
			return false;
		}


	}
}