using System;

namespace RD.ECS.Components
{
	[AttributeUsage( AttributeTargets.Struct, AllowMultiple = true, Inherited = false )]
	public sealed class OneShotAttribute : Attribute { }

}