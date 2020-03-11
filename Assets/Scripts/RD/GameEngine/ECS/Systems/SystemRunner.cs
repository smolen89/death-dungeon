using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RD.GameEngine.ECS.Systems
{
	public sealed class SystemRunner<TState> : IDisposable
	{
		internal static readonly SystemRunner<TState> Default = new SystemRunner<TState>( 1 );

		private readonly CancellationTokenSource disposeHandle;
		private readonly WorkerBarrier barrier;
		private readonly Task[] tasks;

		private ASystem<TState> currentSystem;

		/// <summary>
		/// Initialises a new instance of the <see cref="SystemRunner{T}"/> class.
		/// </summary>
		/// <param name="degreeOfParallelism">The number of <see cref="Task"/> instances used to update a system in parallel.</param>
		/// <exception cref="ArgumentException"><paramref name="degreeOfParallelism"/> cannot be inferior to one.</exception>
		public SystemRunner( int degreeOfParallelism )
		{
			IEnumerable<int> indices = degreeOfParallelism >= 1
				? Enumerable.Range( 0, degreeOfParallelism - 1 )
				: throw new ArgumentException( "Argument cannot be inferior to one", nameof(degreeOfParallelism) );

			disposeHandle = new CancellationTokenSource();
			tasks = indices.Select( index => new Task( Update, index, TaskCreationOptions.LongRunning ) ).ToArray();
			barrier = degreeOfParallelism > 1
				? new WorkerBarrier( tasks.Length )
				: null;

			foreach ( Task task in tasks )
			{
				task.Start( TaskScheduler.Default );
			}
		}

		private void Update( object state )
		{
			int index = (int) state;
			goto Start;

			Work:
			Volatile.Read( ref currentSystem ).Update( index, tasks.Length );
			barrier.Signal();
			
			Start:
			barrier.Start();

			if ( !disposeHandle.IsCancellationRequested )
				goto Work;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Update( ASystem<TState> system )
		{
			Volatile.Write( ref currentSystem,system );
			barrier?.StartWorkers();
			system.Update( tasks.Length, tasks.Length );
			barrier?.WaitForWorkers();
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
			disposeHandle.Cancel();
			barrier?.StartWorkers();
			Task.WaitAll( tasks );
			barrier?.Dispose();
			disposeHandle.Dispose();
		}
	}
}