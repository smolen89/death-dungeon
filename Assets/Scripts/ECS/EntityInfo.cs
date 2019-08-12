using System;
using System.Collections.Generic;
using RD.ECS.Components;

namespace RD.ECS
{
	public struct EntityInfo
	{
		public HashSet<int> Children;
		public Func<int, bool> Parents;
		public IComponent[] Components;
	}
}