using System;
using RD.GameEngine.ECS.Managers;

namespace RD.GameEngine.ECS.Systems.Attributes
{
	[AttributeUsage(AttributeTargets.Class,Inherited = false,AllowMultiple = true)]
	public sealed class EntitySystemAttribute : Attribute
	{
		public EntitySystemAttribute( )
		{
			GameLoopType = GameLoopType.Update;
			Layer = 0;
			ExecutionType = ExecutionType.Sync;
		}
		public GameLoopType GameLoopType { get; set; }
		public int Layer { get; set; }
		public ExecutionType ExecutionType { get; set; }
	}
}