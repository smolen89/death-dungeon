using UnityEngine;
using System.Collections;
using RD.ECS.Message;
using System;
using RD.ECS.Entities;
using System.Collections.Generic;
using RD.ECS.Systems;
using RD.ECS.Components;

namespace RD.ECS
{
	public class World : IPublisher, IDisposable
	{
		public IDisposable RegisterListener<T>( ActionIn<T> action ) => Publisher<T>.RegisterListener( action );

		public void Publish<T>( in T message ) where T : IMessage => Publisher<T>.Publish( message );

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}