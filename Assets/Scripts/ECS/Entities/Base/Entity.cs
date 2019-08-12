using System;
using System.Collections.Generic;
using RD.ECS.Components;
using RD.ECS.Message;

namespace RD.ECS.Entities
{
	public class Entity : IEntity
	{
		private int worldID;
		private int id;
		private string Name;
		private bool isEnabled;
		private List<IComponent> components;

		public Entity( int worldID, int id )
		{
			this.worldID = worldID;
			this.id = id;
		}

		public void Initialize( int entityID )
		{
			id = entityID;
			isEnabled = true;
		}

		public int ID => id;
		public bool IsEnabled => isEnabled;
		public int WorldID => worldID;

		public void Dispose()
		{
			components.Clear();
		}

		public bool Equals( IEntity other ) => throw new NotImplementedException();

		public void Reactivate( int newID ) => throw new NotImplementedException();

		public Func<int, bool> ParentEntity { get; set; }
		public HashSet<int> ChildEntityID { get; set; }
		public int totalComponents { get; }

		public void AddComponent<TComponent>( in TComponent componentData = default ) where TComponent : IComponent => throw new NotImplementedException();

		public void RemoveComponent<TComponent>() where TComponent : IComponent => throw new NotImplementedException();

		public void RemoveAllComponents() => throw new NotImplementedException();

		public TComponent GetComponent<TComponent>() where TComponent : IComponent => throw new NotImplementedException();

		public IComponent[] GetComponents() => throw new NotImplementedException();

		public bool HasComponent<TComponent>() where TComponent : IComponent => throw new NotImplementedException();

		public bool HasComponent( Type componentType ) => throw new NotImplementedException();
	}
}