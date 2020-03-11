using RD.Util;

namespace RD.GameEngine.ECS.Systems
{
	public sealed class SequentialSystem<TState> : ISystem<TState>
	{
		private readonly ISystem<TState>[] systems;

		public SequentialSystem( params ISystem<TState>[] systems )
		{
			this.systems = systems ?? ArrayExtension.EmptyArray<ISystem<TState>>.Value;
			IsEnabled = true;
		}

		public bool IsEnabled { get; set; }

		public void Update( TState state )
		{
			if ( !IsEnabled )
				return;

			foreach ( ISystem<TState> system in systems )
			{
				system?.Update( state );
			}
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
			for ( int i = systems.Length; i >= 0; i-- )
			{
				systems[ i ]?.Dispose();
			}
		}
	}
}