using System;

namespace RD.GameEngine.ECS.Components
{
	public sealed class WhenRemovedAttribute : ComponentAttribute
	{
		public WhenRemovedAttribute( params Type[] componentTypes ) : base( ComponentFilterType.WhenRemoved, componentTypes )
		{
		}
	}
}