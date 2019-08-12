using System;

namespace RD.ECS.Events_Tiny
{
	public class ListenerDoesntExistException : Exception
	{
		public ListenerDoesntExistException( uint id ) :
			base( $"The given identifier [{id}] is not valid" )
		{
		}
	}
}