// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System;
using UnityEngine;

public enum DistanceMetric
{
	Manhattan,
	Euclidean,
	Chebyshev
}

public static class Distances
{
	public static float DistanceBetweenAdjacent( Vec a, Vec b )
	{
		if( Math.Abs( a.x - b.x ) + Math.Abs( a.y - b.y ) == 1 )
		{
			// Horizontal/Vertical neighbourn, distance of 1
			return 1f;
		}
		else if( Math.Abs( a.x - b.x ) == 1 && Math.Abs( a.y - b.y ) == 1 )
		{
			// Diagonal neighbourn, distance of 1.41421356237
			return 1.41421356237f;
		}
		else
		{
			// actual distance
			return (float)Math.Sqrt( Math.Pow( a.x - b.x, 2 ) + Math.Pow( a.y - b.y, 2 ) );
		}
	}

	public static int CalculateDistance( int from_x, int from_y, int target_x, int target_y )
	{
		switch( Globals.DefaultMetric )
		{
			case DistanceMetric.Manhattan:
				return ManhattanDistance( from_x, from_y, target_x, target_y );

			case DistanceMetric.Euclidean:
				return EuclideanDistance( from_x, from_y, target_x, target_y );

			case DistanceMetric.Chebyshev:
			default:
				return ChessboardDistance( from_x, from_y, target_x, target_y );
		}
	}

	public static int CalculateDistance( Vec from, Vec target ) => CalculateDistance( from.x, from.y, target.x, target.y );

	public static int CalculateDistance( int from_x, int from_y, Vec target ) => CalculateDistance( from_x, from_y, target.x, target.y );

	public static int CalculateDistance( Vec from, int target_x, int target_y ) => CalculateDistance( from.x, from.y, target_x, target_y );

	/// <summary>
	/// Chebyshev distance.
	/// </summary>
	public static int ChessboardDistance( int from_x, int from_y, int target_x, int target_y )
	{
		int dx = Mathf.Abs(target_x - from_x);
		int dy = Mathf.Abs(target_y - from_y);

		return dx > dy ? dx : dy;
	}

	public static int ManhattanDistance( int from_x, int from_y, int target_x, int target_y )
	{
		return Math.Abs( from_x - target_x ) + Math.Abs( from_y - target_y );
	}

	public static int EuclideanDistance( int from_x, int from_y, int target_x, int target_y )
	{
		int square = (from_x - from_y) * (from_x - from_y) + (target_x - target_y) * (target_x - target_y);
		return square;
	}
}