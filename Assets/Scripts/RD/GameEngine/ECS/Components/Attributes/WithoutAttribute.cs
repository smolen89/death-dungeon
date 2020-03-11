using System;

namespace RD.GameEngine.ECS.Components
{
	public sealed class WithoutAttribute : ComponentAttribute
	{
		public WithoutAttribute( params Type[] componentTypes ) : base( ComponentFilterType.Without, componentTypes )
		{
		}
	}
}