namespace RD.ECS.Events_Tiny
{
	public struct EntityDestroyedEvent : IEvent
	{
		public uint EntityID;
		public string EntityName;
	}
}