using System;

namespace RD.ECS.Components
{
	public class InvalidIteratorException : Exception
	{
		public InvalidIteratorException() :
			base( "Iterator to a particular component is invalid" )
		{
		}
	}
}