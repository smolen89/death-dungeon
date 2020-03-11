using System;

namespace RD.GameEngine.ECS.Components
{
	public sealed class WithAttribute : ComponentAttribute
	{
		public WithAttribute( params Type[] componentTypes ) : base( ComponentFilterType.With, componentTypes )
		{
		}
	}
}