using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RD.ECS.Systems
{
	public sealed class SystemRunner<TUpdate> : IDisposable
	{
		internal static SystemRunner<TUpdate> Default = new SystemRunner<TUpdate>( 1 );

		private readonly CancellationTokenSource disposeHandle;
		private readonly WorkerBarrier barrier;
		private readonly Task[] tasks;

		private System<TUpdate> currentSystem;

		public SystemRunner( int degreeOfParallelism )
		{
			IEnumerable<int> indices = degreeOfParallelism >= 1
				? Enumerable.Range( 0, degreeOfParallelism - 1 )
				: throw new ArgumentException( "Argument cannot be inferior to one", nameof( degreeOfParallelism ) );

			disposeHandle = new CancellationTokenSource();
			tasks = indices.Select( index => new Task( Update, index, TaskCreationOptions.LongRunning ) ).ToArray();
			barrier = degreeOfParallelism > 1
				? new WorkerBarrier( tasks.Length )
				: null;
			foreach( Task task in tasks )
			{
				task.Start( TaskScheduler.Default );
			}
		}

		private void Update( object state )
		{
			int index = (int)state;
			goto Start;

			Work:
			Volatile.Read( ref currentSystem ).Update( index, tasks.Length );
			barrier.Signal();

			Start:
			barrier.Start();
			if( !disposeHandle.IsCancellationRequested )
			{
				goto Work;
			}
		}

		internal void Update( System<TUpdate> system )
		{
			Volatile.Write( ref currentSystem, system );
			barrier?.StartWorkers();
			system.Update( tasks.Length, tasks.Length );
			barrier?.WaitForWorkers();
		}

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