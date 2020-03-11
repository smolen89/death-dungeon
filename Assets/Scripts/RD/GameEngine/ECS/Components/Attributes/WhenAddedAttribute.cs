using System;

namespace RD.GameEngine.ECS.Components
{
	public sealed class WhenAddedAttribute : ComponentAttribute
	{
		public WhenAddedAttribute( params Type[] componentTypes ) : base( ComponentFilterType.WhenAdded, componentTypes )
		{
		}
	}
}