using System;

namespace RD
{
	/// <summary>
	/// Podstawowy Exception.
	/// </summary>
	public class RDException : Exception
	{
		public RDException( string message, string hint ) : base( hint != null ? ( message + "\n" + hint ) : message ) { }
	}
}