using System;
using System.Runtime.Serialization;

namespace RD.Exceptions
{
	[Serializable]
	public class MissingEntityTemplateException : Exception
	{
		internal MissingEntityTemplateException( string entityTemplateTag ) 
			: this( entityTemplateTag, null )
		{
		}

		internal MissingEntityTemplateException( string entityTemplateTag, Exception inner )
			: base( $"EntityTemplate for the tag {entityTemplateTag} was not registered.", inner )
		{
		}

		protected MissingEntityTemplateException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
			
		}
	}
}