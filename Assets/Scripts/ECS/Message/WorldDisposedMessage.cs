namespace RD.ECS.Message
{
	public readonly struct WorldDisposedMessage : IMessage
	{
		public readonly int WorldID;

		public WorldDisposedMessage( int worldID ) => this.WorldID = worldID;
	}
}