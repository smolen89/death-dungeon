using System;

namespace RD.GameEngine.ECS.Components
{
	public sealed class WithoutEitherAttribute : ComponentAttribute
	{
		public WithoutEitherAttribute( params Type[] componentTypes ) : base( ComponentFilterType.WithoutEither, componentTypes )
		{
		}
	}
}