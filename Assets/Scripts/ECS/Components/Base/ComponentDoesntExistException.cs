using System;

namespace RD.ECS.Components
{
	public partial class ComponentManager
	{
		public class ComponentDoesntExistException : Exception
		{
			public ComponentDoesntExistException( Type type, uint entityId ) :
				base( $"A component of [{type}] doesn't belong to entity with id [{entityId}]" )
			{
			}
		}
	}
}