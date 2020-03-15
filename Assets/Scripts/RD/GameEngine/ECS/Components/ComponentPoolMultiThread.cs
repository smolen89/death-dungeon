using System;

namespace RD.GameEngine.ECS.Components
{
	public class ComponentPoolMultiThread<T> : IComponentPool<T>
	where T : ComponentPoolable
	{
		private readonly ComponentPool<T> pool;
		private readonly object sync;

		public ComponentPoolMultiThread( int initialSize, int resizePool, bool resizes, Func<Type, T> allocateFunc, Type innerType )
		{
			pool = new ComponentPool<T>( initialSize, resizePool, resizes, allocateFunc, innerType );
			sync = new object();
		}

		public int InvalidCount => pool.InvalidCount;

		public int ResizeAmount => pool.ResizeAmount;

		public int ValidCount => pool.ValidCount;

		public T this[ int index ]
		{
			get
			{
				lock ( sync )
				{
					return pool[ index ];
				}
			}
		}

		public void CleanUp()
		{
			lock ( sync )
			{
				pool.CleanUp();
			}
		}

		public T New()
		{
			lock ( sync )
			{
				return pool.New();
			}
		}

		public void ReturnObject( T component )
		{
			lock ( sync )
			{
				pool.ReturnObject( component );
			}
		}
	}
}