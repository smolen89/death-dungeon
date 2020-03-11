namespace RD.GameEngine.ECS.Components
{
	public abstract class ComponentPoolable : IComponent
	{
		protected ComponentPoolable( )
		{
			PoolId = 0;
		}

		internal int PoolId { get; set; }

		public virtual void CleanUp()
		{
		}

		public virtual void Initialize()
		{
		}
	}
}