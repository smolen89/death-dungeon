namespace RD.ECS.Systems
{
	public abstract class System<TUpdate> : ISystemUpdate<TUpdate>
	{
		private readonly SystemRunner<TUpdate> systemRunner;
		internal TUpdate updateState;

		protected System( SystemRunner<TUpdate> systemRunner )
		{
			this.systemRunner = systemRunner ?? SystemRunner<TUpdate>.Default;
			IsEnabled = true;
		}

		protected System() : this( null )
		{
		}

		public abstract void Update( int index, int maxIndex );

		protected virtual void PreUpdate( TUpdate dt )
		{
		}

		protected virtual void PostUpdate( TUpdate dt )
		{
		}

		public virtual void Update( TUpdate dt )
		{
			if( IsEnabled )
			{
				updateState = dt;
				PreUpdate( updateState );
				systemRunner.Update( this );
				PostUpdate( updateState );
			}
		}

		public virtual bool IsEnabled { get; set; }

		public abstract void Dispose();
	}
}