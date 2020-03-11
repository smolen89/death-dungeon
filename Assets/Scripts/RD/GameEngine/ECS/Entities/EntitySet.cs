using System;
using System.Collections.Generic;
using RD.GameEngine.ECS.Components;

namespace RD.GameEngine.ECS.Entities
{
	public sealed  class EntitySet : IDisposable
	{
		private readonly bool needClearing;
		private readonly int worldId;
		private readonly int maxEntityCount;
		private readonly Predicate<List<IComponent>> filters;

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
		}
	}
}