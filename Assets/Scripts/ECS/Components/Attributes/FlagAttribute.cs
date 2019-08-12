using System;

namespace RD.ECS.Components
{
	[AttributeUsage( AttributeTargets.Struct, AllowMultiple = true, Inherited = false )]
	public sealed class FlagAttribute : Attribute
	{
		public readonly string prefix;

		public FlagAttribute( string prefix )
		{
			this.prefix = prefix ?? throw new ArgumentNullException( nameof( prefix ) );
		}
	}
}