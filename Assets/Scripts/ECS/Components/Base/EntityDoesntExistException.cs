using System;

namespace RD.ECS.Components
{
	public class EntityDoesntExistException : Exception
	{
		public EntityDoesntExistException( uint entityId ) :
			base( $"An entity with the specified identifier [{entityId}] doesn't exist" )
		{
		}
	}

}