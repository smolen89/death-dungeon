using System;

namespace RD.GameEngine.ECS.Components
{
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
	public class ComponentAttribute : Attribute
	{
		public readonly Type[] ComponentTypes;
		public readonly ComponentFilterType FilterType;

		public ComponentAttribute( ComponentFilterType filterType, params Type[] componentTypes )
		{
			ComponentTypes = componentTypes ?? throw new ArgumentNullException( nameof(componentTypes) );
			FilterType = filterType;
		}
	}
}