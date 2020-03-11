using System;

namespace RD.GameEngine.ECS.Components
{
	public sealed class WithEitherAttribute : ComponentAttribute
	{
		public WithEitherAttribute( params Type[] componentTypes ) : base( ComponentFilterType.WithEither, componentTypes )
		{
		}
	}
}