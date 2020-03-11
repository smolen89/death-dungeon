using System;

namespace RD.GameEngine.ECS.Systems
{
	public sealed class ActionSystem<TState> : ISystem<TState>
	{
		private readonly Action<TState> action;

		public ActionSystem( Action<TState> action )
		{
			this.action = action ?? throw new ArgumentNullException( nameof(action) );
			IsEnabled = true;
		}

		public bool IsEnabled { get; set; }

		public void Update( TState state )
		{
			if ( !IsEnabled ) 
				return;

			action( state );
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
		}
	}
}