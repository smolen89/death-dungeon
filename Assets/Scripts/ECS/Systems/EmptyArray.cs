namespace RD.ECS.Systems
{
	internal static class EmptyArray<T>
	{
#if NETSTANDARD1_1
        public static T[] Value { get; } = new T[0];
#else
		public static T[] Value => System.Array.Empty<T>();
#endif
	}
}