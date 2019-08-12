using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public static class IDisposableExtension
{
	private sealed class DisposableGroup : IDisposable
	{
		private readonly IDisposable[] disposables;

		public DisposableGroup( IEnumerable<IDisposable> disposables )
		{
			this.disposables = GetDisposables( disposables ).ToArray();
		}

		private IEnumerable<IDisposable> GetDisposables( IEnumerable<IDisposable> disposables )
		{
			foreach( IDisposable disposable in disposables )
			{
				if( disposable is DisposableGroup group )
				{
					foreach( IDisposable child in group.disposables )
					{
						yield return child;
					}
				}
				else
				{
					yield return disposable;
				}
			}
		}

		void IDisposable.Dispose()
		{
			for( int i = disposables.Length - 1; i >= 0; i-- )
			{
				disposables[ i ].Dispose();
			}
			GC.SuppressFinalize( this );
		}
	}

	public static IDisposable Merge( this IEnumerable<IDisposable> disposables ) => new DisposableGroup( disposables ?? Enumerable.Empty<IDisposable>() );
}