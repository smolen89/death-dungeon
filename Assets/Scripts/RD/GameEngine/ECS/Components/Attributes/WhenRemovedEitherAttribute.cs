using System;

namespace RD.GameEngine.ECS.Components
{
	public sealed class WhenRemovedEitherAttribute : ComponentAttribute
	{
		public WhenRemovedEitherAttribute( params Type[] componentTypes ) : base( ComponentFilterType.WhenRemovedEither, componentTypes )
		{
		}
	}
}