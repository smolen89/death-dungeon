using System;

namespace RD.GameEngine.ECS.Systems.Attributes
{
	[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = false )]
	public sealed class ComponentPoolAttribute : Attribute
	{
		public ComponentPoolAttribute()
		{
			InitialSize = 10;
			ResizeSize = 10;
			IsResisable = true;
			IsSupportMultiThread = false;
		}

		public int InitialSize { get; set; }
		public int ResizeSize { get; set; }
		public bool IsResisable { get; set; }
		public bool IsSupportMultiThread { get; set; }
	}
}