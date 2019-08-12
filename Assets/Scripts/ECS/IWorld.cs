using RD.ECS.Components;
using RD.ECS.Entities;
using RD.ECS.Message;
using System;
using System.Collections.Generic;

namespace RD.ECS
{
	public interface IWorld : IPublisher, IDisposable
	{
		int totalComponents { get; }
		Stack<IComponent>[] componentPool { get; }
		int count { get; }
		int reusableEntitiesCount { get; }
		int retainedEntitiesCount { get; }

		void DestroyAllEntities();

		void ClearComponentPool( int componentIndex );

		void ClearComponentPools();

		void Reset();
	}

	public interface IWorld<TEntity> : IWorld where TEntity : class, IEntity
	{
		TEntity CreateEntity();

		bool HasEntity( TEntity entity );

		TEntity[] GetEntities();

		//todo EntitySet<TEntity> GetGroup(param IComponent[] filter);
	}
}