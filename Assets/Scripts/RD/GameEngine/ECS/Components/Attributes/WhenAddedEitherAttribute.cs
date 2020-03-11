using System;

namespace RD.GameEngine.ECS.Components
{
	public sealed class WhenAddedEitherAttribute : ComponentAttribute
	{
		public WhenAddedEitherAttribute( params Type[] componentTypes ) : base( ComponentFilterType.WhenAddedEither, componentTypes )
		{
		}
	}
}