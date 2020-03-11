namespace RD.GameEngine.ECS.Systems
{
	public abstract class ASystem<TState> : ISystem<TState>
	{
		private readonly SystemRunner<TState> runner;
		internal TState CurrentState;

		protected ASystem( SystemRunner<TState> runner )
		{
			this.runner = runner ?? SystemRunner<TState>.Default;
		}

		protected ASystem() : this( null )
		{
		}

		internal abstract void Update( int index, int maxIndex );

		protected virtual void PreUpdate( TState state )
		{
		}

		protected virtual void PostUpdate( TState state )
		{
		}

		public virtual bool IsEnabled { get; set; } = true;

		public void Update( TState state )
		{
			if ( !IsEnabled ) 
				return;

			CurrentState = state;
			PreUpdate( CurrentState );
			runner.Update( this );
			PostUpdate( CurrentState );
		}

		public abstract void Dispose();
	}
}