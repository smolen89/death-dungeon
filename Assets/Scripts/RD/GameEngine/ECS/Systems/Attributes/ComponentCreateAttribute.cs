using System;

namespace RD.GameEngine.ECS.Systems.Attributes
{
	[AttributeUsage( AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
	public sealed class ComponentCreateAttribute : Attribute
	{
	}
}