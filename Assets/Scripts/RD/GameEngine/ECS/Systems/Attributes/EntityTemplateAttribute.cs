using System;

namespace RD.GameEngine.ECS.Systems.Attributes
{
	[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = true )]
	public sealed class EntityTemplateAttribute : Attribute
	{
		public EntityTemplateAttribute( string name )
		{
			Name = name;
		}
		public string Name { get; private set; }
	}
}