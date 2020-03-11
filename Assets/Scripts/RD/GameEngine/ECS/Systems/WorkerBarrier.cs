using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RD.GameEngine.ECS.Systems
{
	internal sealed class WorkerBarrier : IDisposable
	{
		private readonly int count;
		private readonly ManualResetEventSlim endHandle;
		private readonly ManualResetEventSlim startHandle;

		private bool allStarted;
		private int runningCount;

		public WorkerBarrier( int workerCount )
		{
			count = workerCount;
			endHandle = new ManualResetEventSlim( false );
			startHandle = new ManualResetEventSlim( false );

			allStarted = false;
			runningCount = 0;
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public void StartWorkers()
		{
			Volatile.Write( ref allStarted, false );
			startHandle.Set();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public void Start()
		{
			startHandle.Wait();

			if ( Interlocked.Increment( ref runningCount ) == count )
			{
				startHandle.Reset();
				Volatile.Write( ref allStarted, true );
			}
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public void Signal()
		{
			while ( !Volatile.Read( ref allStarted ) )
			{
			}

			if ( Interlocked.Decrement( ref runningCount ) == 0 )
			{
				endHandle.Set();
			}
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public void WaitForWorkers()
		{
			endHandle.Wait();
			endHandle.Reset();
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
			endHandle.Dispose();
			startHandle.Dispose();
		}
	}
}