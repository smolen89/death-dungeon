namespace RD.ECS.Contexts
{
	public struct ContextStats
	{
		public uint ActiveEntitiesCount;
		public uint ReservedEntitiesCount;
		public uint ActiveComponents;
		public uint AverageCountComponentsPerEntity;
	}
}
