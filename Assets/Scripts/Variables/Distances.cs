// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System;
using UnityEngine;

public enum DistanceMetric
{
	/// <summary>
	/// Nie liczy skosów, tylko góra dół oraz lewo i prawo.
	/// </summary>
	Manhattan,
	/// <summary>
	/// To będzie to
	/// </summary>
	Euclidean,
	/// <summary>
	/// Chessboard distance
	/// </summary>
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

	public static int CalculateDistance(int fromX, int fromY, int targetX, int targetY )
	{
		switch( Globals.DefaultMetric )
		{
			case DistanceMetric.Manhattan:
				return ManhattanDistance( fromX, fromY, targetX, targetY );

			case DistanceMetric.Euclidean:
				return EuclideanDistance( fromX, fromY, targetX, targetY );

			//case DistanceMetric.Chebyshev:
			default:
				return ChessboardDistance( fromX, fromY, targetX, targetY );
		}
	}

	public static int CalculateDistance( Vec from, Vec target ) => CalculateDistance( from.x, from.y, target.x, target.y );

	public static int CalculateDistance( int fromX, int fromY, Vec target ) => CalculateDistance( fromX, fromY, target.x, target.y );

	public static int CalculateDistance( Vec from, int targetX, int targetY ) => CalculateDistance( from.x, from.y, targetX, targetY );

	/// <summary>
	/// Chebyshev distance.
	/// </summary>
	public static int ChessboardDistance( int fromX, int fromY, int targetX, int targetY )
	{
		int dx = Mathf.Abs(targetX - fromX);
		int dy = Mathf.Abs(targetY - fromY);

		return dx > dy ? dx : dy;
	}

	public static int ManhattanDistance( int fromX, int fromY, int targetX, int targetY )
	{
		return Math.Abs( fromX - targetX ) + Math.Abs( fromY - targetY );
	}

	public static int EuclideanDistance( int fromX, int fromY, int targetX, int targetY )
	{
		int square = (fromX - fromY) * (fromX - fromY) + (targetX - targetY) * (targetX - targetY);
		return square;
	}
}