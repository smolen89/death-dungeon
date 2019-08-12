using System;
using System.Threading;

namespace RD.ECS.Systems
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
			this.count = workerCount;
			endHandle = new ManualResetEventSlim( false );
			startHandle = new ManualResetEventSlim( false );

			allStarted = false;
			runningCount = 0;
		}

		public void StartWorkers()
		{
			Volatile.Write( ref allStarted, false );
			startHandle.Set();
		}

		public void Start()
		{
			startHandle.Wait();
			if( Interlocked.Increment( ref runningCount ) == count )
			{
				startHandle.Reset();
				Volatile.Write( ref allStarted, true );
			}
		}

		public void Signal()
		{
			while( !Volatile.Read( ref allStarted ) )
			{
			}
			if( Interlocked.Decrement( ref runningCount ) == 0 )
			{
				endHandle.Set();
			}
		}

		public void WaitForWorkers()
		{
			endHandle.Wait();
			endHandle.Reset();
		}

		public void Dispose()
		{
			endHandle.Dispose();
			startHandle.Dispose();
		}
	}
}