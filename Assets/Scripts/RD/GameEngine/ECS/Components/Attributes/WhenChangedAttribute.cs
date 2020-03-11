using System;

namespace RD.GameEngine.ECS.Components
{
	public sealed class WhenChangedAttribute : ComponentAttribute
	{
		public WhenChangedAttribute( params Type[] componentTypes ) : base( ComponentFilterType.WhenChanged, componentTypes )
		{
		}
	}
}