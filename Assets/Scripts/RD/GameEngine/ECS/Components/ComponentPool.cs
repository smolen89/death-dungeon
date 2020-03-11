using System;
using System.Collections.Generic;

namespace RD.GameEngine.ECS.Components
{
	public class ComponentPool<T> : IComponentPool<T> where T : ComponentPoolable
	{
		private readonly Func<Type, T> allocate;
		private readonly bool isResizeAllowed;
		private readonly Type innerType;
		private readonly List<T> invalidComponents;
		
		private T[] components;

		public ComponentPool( int initialSize, int resizePool, bool resizes, Func<Type, T> allocateFunc, Type innerType )
		{
			invalidComponents = new List<T>();
			
			components = initialSize < 1
				? throw new ArgumentOutOfRangeException( nameof(initialSize), $"Must be at least 1." )
				: new T[initialSize];

			ResizeAmount = resizePool < 1
				? throw new ArgumentOutOfRangeException( nameof(resizePool), $"must be at least 1." )
				: resizePool;

			allocate = allocateFunc ?? throw new ArgumentNullException( nameof(allocateFunc) );

			this.innerType = innerType ?? throw new ArgumentNullException( nameof(innerType) );
			
			isResizeAllowed = resizes;
			InvalidCount = components.Length;
		}

		public int InvalidCount { get; private set; }
		public int ResizeAmount { get; internal set; }

		public int ValidCount => components.Length - InvalidCount;

		public T this[ int index ]
		{
			get
			{
				index += InvalidCount;

				if ( index < InvalidCount || index >= components.Length )
					throw new ArgumentOutOfRangeException( nameof(index), $"The index must be less than or equal to ValidCount" );

				return components[ index ];
			}
		}

		public T New()
		{
			if ( InvalidCount == 0 )
			{
				if ( isResizeAllowed )
				{
					throw new Exception( $"Limit Exceeded {components.Length}, and the pool was set to not resize." );
				}

				T[] newComponents = new T[components.Length + ResizeAmount];

				for ( int index = components.Length - 1; index >= 0; --index )
				{
					if ( index >= InvalidCount )
					{
						components[ index ].PoolId = index + ResizeAmount;
					}

					newComponents[ index + ResizeAmount ] = components[ index ];
				}

				components = newComponents;
				InvalidCount += ResizeAmount;
			}

			InvalidCount--;
			T result = components[ InvalidCount ];

			if ( result == null )
			{
				result = allocate( innerType );
				components[ InvalidCount ] = result ?? throw new InvalidOperationException( $"The pool's allocated method returned a null object reference." );
			}

			result.PoolId = InvalidCount;
			result.Initialize();

			return result;
		}

		public void CleanUp()
		{
			foreach ( T component in invalidComponents )
			{
				if ( component.PoolId != InvalidCount )
				{
					components[ component.PoolId ] = components[ InvalidCount ];
					components[ InvalidCount ].PoolId = component.PoolId;
					components[ InvalidCount ] = component;
					component.PoolId = -1;
				}

				component.CleanUp();
				InvalidCount++;
			}

			invalidComponents.Clear();
		}

		public void ReturnObject( T component )
		{
			invalidComponents.Add( component );
		}
	}
}