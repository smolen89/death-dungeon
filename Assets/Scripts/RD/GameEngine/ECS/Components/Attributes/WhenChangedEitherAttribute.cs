using System;

namespace RD.GameEngine.ECS.Components
{
	public sealed class WhenChangedEitherAttribute : ComponentAttribute
	{
		public WhenChangedEitherAttribute( params Type[] componentTypes ) : base( ComponentFilterType.WhenChangedEither, componentTypes )
		{
		}
	}
}