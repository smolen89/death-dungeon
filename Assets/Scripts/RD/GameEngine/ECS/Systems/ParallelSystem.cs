using System.Threading;
using RD.Util;

namespace RD.GameEngine.ECS.Systems
{
	public sealed class ParallelSystem<TState> : ASystem<TState>
	{
		private readonly ISystem<TState> mainSystem;
		private readonly ISystem<TState>[] systems;

		private int lastIndex;

		public ParallelSystem( ISystem<TState> mainSystem, SystemRunner<TState> runner, params ISystem<TState>[] systems ) : base( runner )
		{
			this.mainSystem = mainSystem;
			this.systems = systems ?? ArrayExtension.EmptyArray<ISystem<TState>>.Value;
		}

		public ParallelSystem( SystemRunner<TState> runner, params ISystem<TState>[] systems ) : this( null, runner, systems )
		{
		}

		internal override void Update( int index, int maxIndex )
		{
			if ( index == maxIndex )
			{
				mainSystem?.Update( CurrentState );
			}

			while ( ( index = Interlocked.Increment( ref lastIndex ) ) < systems.Length )
			{
				systems[ index ]?.Update( CurrentState );
			}
		}

		protected override void PreUpdate( TState state )
		{
			Interlocked.Exchange( ref lastIndex, -1 );
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public override void Dispose()
		{
			mainSystem?.Dispose();

			foreach ( ISystem<TState> system in systems )
			{
				system?.Dispose();
			}
		}
	}
}