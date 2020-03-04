// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System;
using System.Collections.Generic;

public static class VecExtensions
{
	#region Distance

	public static int CalculateDistance( this Vec source, int destinationX, int destinationY ) => Distances.CalculateDistance( source, destinationX, destinationY );

	public static int CalculateDistance( this Vec source, Vec destination ) => Distances.CalculateDistance( source, destination );


	/// <summary>
	/// Gets whether the distance between the two given <see cref="Vec">Vecs</see> is within
	/// the given distance.
	/// </summary>
	/// <param name="b">Other Vec.</param>
	/// <param name="distance">Maximum distance between.</param>
	/// <returns><c>true</c> if the distance between <c>a</c> and <c>b</c> is less than or equal to <c>distance</c>.</returns>
	public static bool IsDistanceWithin( this Vec a, Vec b, int distance ) => ( a - b ).LengthSquared <= ( distance * distance );
	#endregion Distance

	#region Positions

	public static List<Vec> CalculatePositionsWithinDistance<T>( this Vec source, int distance, VecArray<T> array, bool excludeOrigin = false )
	{
		List<Vec> result = new List<Vec>();
		for( int x = source.x - distance; x <= source.x + distance; ++x )
		{
			for( int y = source.y - distance; y <= source.y + distance; ++y )
			{
				if( !excludeOrigin || x != source.x || y != source.y )
				{
					if( Distances.CalculateDistance( source.x, source.y, x, y ) <= distance )
						if( array == null || array.BoundsCheck( x, y ) )
						{
							result.Add( new Vec( x, y ) );
						}
				}
			}
		}
		return result;
	}

	public static List<Vec> PositionsWithinDistance<T>( this Vec source, int distance, VecArray<T> array, bool excludeOrigin = false )
	{
		switch( Globals.DefaultMetric )
		{
			case DistanceMetric.Manhattan:
				return source.PositionsWithinManhattanDistance( distance, array, excludeOrigin );

			case DistanceMetric.Euclidean:
				return source.PositionsWithinEuclideanDistance( distance, array, excludeOrigin );

			//case DistanceMetric.Chebyshev:
			default:
				return source.PositionsWithinChebyshevDistance( distance, array, excludeOrigin );
		}
	}

	public static List<Vec> PositionsWithinDistance( this Vec source, int distance, bool excludeOrigin = false ) => source.PositionsWithinDistance<int>( distance, null, excludeOrigin );

	public static List<Vec> PositionsAtDistance<T>( this Vec source, int distance, VecArray<T> array )
	{
		switch( Globals.DefaultMetric )
		{
			case DistanceMetric.Manhattan:
				return source.PositionsAtManhattanDistance( distance, array );

			case DistanceMetric.Euclidean:
				return source.PositionsAtEuclideanDistance( distance, array );

			//case DistanceMetric.Chebyshev:
			default:
				return source.PositionsAtChebyshevDistance( distance, array );
		}
	}

	public static List<Vec> PositionsAtDistance( this Vec source, int distance ) => source.PositionsAtDistance<int>( distance, null );

	#region Chebyshev

	public static List<Vec> PositionsWithinChebyshevDistance<T>( this Vec source, int distance, VecArray<T> array, bool excludeOrigin = false )
	{
		List<Vec> result = new List<Vec>();
		for( int x = source.x - distance; x <= source.x + distance; ++x )
		{
			for( int y = source.y - distance; y <= source.y + distance; ++y )
			{
				if( !excludeOrigin || x != source.x || y != source.y )
				{
					if( Distances.ChessboardDistance( source.x, source.y, x, y ) <= distance )
						if( array == null || array.BoundsCheck( x, y ) )
						{
							result.Add( new Vec( x, y ) );
						}
				}
			}
		}
		return result;
	}

	public static List<Vec> PositionsWithinChebyshevDistance( this Vec source, int distance, bool excludeOrigin = false ) => source.PositionsWithinChebyshevDistance<int>( distance, null, excludeOrigin );

	public static List<Vec> PositionsAtChebyshevDistance<T>( this Vec source, int distance, VecArray<T> array )
	{
		List<Vec> result = new List<Vec>();
		for( int x = source.x - distance; x <= source.x + distance; ++x )
		{
			for( int y = source.y - distance; y <= source.y + distance; ++y )
			{
				if( Distances.ChessboardDistance( source.x, source.y, x, y ) == distance )
				{
					if( array == null || array.BoundsCheck( x, y ) )
					{
						result.Add( new Vec( x, y ) );
					}
				}
				else
				{
					y = source.y + distance - 1;
				}
			}
		}
		return result;
	}

	public static List<Vec> PositionsAtChebyshevDistance( this Vec source, int distance ) => source.PositionsAtChebyshevDistance<int>( distance, null );

	#endregion Chebyshev

	#region Manhattan

	public static List<Vec> PositionsWithinManhattanDistance<T>( this Vec source, int distance, VecArray<T> array, bool excludeOrigin = false )
	{
		List<Vec> result = new List<Vec>();
		for( int x = source.x - distance; x <= source.x + distance; ++x )
		{
			for( int y = source.y - distance; y <= source.y + distance; ++y )
			{
				if( !excludeOrigin || x != source.x || y != source.y )
				{
					if( array == null || array.BoundsCheck( x, y ) )
					{
						if( Distances.ManhattanDistance( source.x, source.y, x, y ) <= distance )
						{ //room for improvement here.
							result.Add( new Vec( x, y ) );
						}
					}
				}
			}
		}
		return result;
	}

	public static List<Vec> PositionsWithinManhattanDistance( this Vec source, int distance, bool excludeOrigin = false ) => source.PositionsWithinManhattanDistance<int>( distance, null, excludeOrigin );

	public static List<Vec> PositionsAtManhattanDistance<T>( this Vec source, int distance, VecArray<T> array )
	{
		List<Vec> result = new List<Vec>();
		for( int x = source.x - distance; x <= source.x + distance; ++x )
		{ //room for improvement99jnm,
			for( int y = source.y - distance; y <= source.y + distance; ++y )
			{
				if( Distances.ManhattanDistance( source.x, source.y, x, y ) == distance )
				{
					if( array == null || array.BoundsCheck( x, y ) )
					{
						result.Add( new Vec( x, y ) );
					}
				}
			}
		}
		return result;
	}

	public static List<Vec> PositionsAtManhattanDistance( this Vec source, int distance ) => source.PositionsAtManhattanDistance<int>( distance, null );

	#endregion Manhattan

	#region Euclidean

	public static List<Vec> PositionsWithinEuclideanDistance<T>( this Vec source, int distance, VecArray<T> array, bool excludeOrigin = false )
	{
		List<Vec> result = new List<Vec>();
		for( int x = source.x - distance; x <= source.x + distance; ++x )
		{
			for( int y = source.y - distance; y <= source.y + distance; ++y )
			{
				if( !excludeOrigin || x != source.x || y != source.y )
				{
					if( array == null || array.BoundsCheck( x, y ) )
					{
						if( Distances.EuclideanDistance( source.x, source.y, x, y ) <= distance )
						{ //room for improvement here.
							result.Add( new Vec( x, y ) );
						}
					}
				}
			}
		}
		return result;
	}

	public static List<Vec> PositionsWithinEuclideanDistance<T>( this Vec source, int distance, bool excludeOrigin = false ) => source.PositionsWithinEuclideanDistance<int>( distance, null, excludeOrigin );

	public static List<Vec> PositionsAtEuclideanDistance<T>( this Vec source, int distance, VecArray<T> array )
	{
		List<Vec> result = new List<Vec>();
		for( int x = source.x - distance; x <= source.x + distance; ++x )
		{ //room for improvement,
			for( int y = source.y - distance; y <= source.y + distance; ++y )
			{
				if( Distances.EuclideanDistance( source.x, source.y, x, y ) == distance )
				{
					if( array == null || array.BoundsCheck( x, y ) )
					{
						result.Add( new Vec( x, y ) );
					}
				}
			}
		}
		return result;
	}

	public static List<Vec> PositionsAtEuclideanDistance( this Vec source, int distance ) => source.PositionsAtEuclideanDistance<int>( distance, null );

	#endregion Euclidean

	[Obsolete( "int dir został zamieniony na Direction dir.", true )]
	public static Vec PositionInDirection( this Vec source, int direction )
	{
		switch( direction )
		{
			case 7:
				return new Vec( source.x - 1, source.y + 1 );

			case 8:
				return new Vec( source.x, source.y + 1 );

			case 9:
				return new Vec( source.x + 1, source.y + 1 );

			case 4:
				return new Vec( source.x - 1, source.y );

			case 5:
				return source;

			case 6:
				return new Vec( source.x + 1, source.y );

			case 1:
				return new Vec( source.x - 1, source.y - 1 );

			case 2:
				return new Vec( source.x, source.y - 1 );

			case 3:
				return new Vec( source.x + 1, source.y - 1 );

			default:
				return new Vec( -1, -1 );
		}
	}

	public static int ConsecutiveAdjacent(this Vec source, Func<Vec,bool> condition )
	{
		int maxCount = 0;
		int count = 0;

		for( int times = 0; times < 2; times++ )
		{
			for( int i = 0; i < 8; i++ )
			{
				if (condition(source + Direction.Up.Rotate8Way( i ) ) )
				{
					count++;
				}
				else
				{
					if( count > maxCount )
					{
						maxCount = count;
					}
					count = 0;
				}
			}
			if (count == 8 )
			{
				return count;
			}
		}
		return maxCount;
	}

	#endregion Positions
}