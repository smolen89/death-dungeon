namespace RD.ECS.Events_Tiny
{
	public struct ComponentChangedEvent<TComponent> : IEvent
	{
		public uint EntityID;
		public TComponent Value;

		public ComponentChangedEvent( uint entityID, TComponent value )
		{
			this.EntityID = entityID;
			this.Value = value;
		}
	}
}