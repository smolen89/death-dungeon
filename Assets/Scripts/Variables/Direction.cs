// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System;
using System.Collections.Generic;

/// <summary>
/// Represents one of the eight directions on a compass (or no direction).
/// </summary>
[Serializable]
public struct Direction : IEquatable<Direction>
{
	private Direction( Vec offset )
	{
		this.offset = offset;
	}

	private Vec offset;
	public Vec Offset => offset;

	#region static directions

	/// <summary>
	/// Gets the "none" direction.
	/// </summary>
	public static Direction None
		=> new Direction( (0, 0) );

	public static Direction Up
		=> new Direction( (0, 1) );

	public static Direction UpRight
		=> new Direction( (1, 1) );

	public static Direction Right
		=> new Direction( (1, 0) );

	public static Direction DownRight
		=> new Direction( (1, -1) );

	public static Direction Down
		=> new Direction( (0, -1) );

	public static Direction DownLeft
		=> new Direction( (-1, -1) );

	public static Direction Left
		=> new Direction( (-1, 0) );

	public static Direction UpLeft
		=> new Direction( (-1, 1) );

	#endregion static directions

	#region Operators

	/// <summary>
	/// Adds the offset of the given Direction to the Vec.
	/// </summary>
	/// <param name="v1">Vector to add the Direction to.</param>
	/// <param name="d2">Direction to offset the vector.</param>
	/// <returns>A new Vec.</returns>
	public static Vec operator +( Vec v1, Direction d2 ) => v1 + d2.Offset;

	/// <summary>
	/// Adds the offset of the given Direction to the Vec.
	/// </summary>
	/// <param name="d1">Direction to offset the vector.</param>
	/// <param name="v2">Vector to add the Direction to.</param>
	/// <returns>A new Vec.</returns>
	public static Vec operator +( Direction d1, Vec v2 ) => d1.Offset + v2;

	public static bool operator ==( Direction left, Direction right ) => left.Equals( right );

	public static bool operator !=( Direction left, Direction right ) => !left.Equals( right );

	#endregion Operators

	/// <summary>
	/// Enumerates the directions in clockwise order, starting with <see cref="Up"/>.
	/// </summary>
	public static IEnumerable<Direction> Clockwise()
	{
		yield return Up;
		yield return UpRight;
		yield return Right;
		yield return DownRight;
		yield return Down;
		yield return DownLeft;
		yield return Left;
		yield return UpLeft;
	}

	/// <summary>
	/// Enumerates the directions in counterclockwise order, starting with <see cref="Up"/>.
	/// </summary>
	public static IEnumerable<Direction> CounterClockwise()
	{
		yield return Up;
		yield return UpLeft;
		yield return Left;
		yield return DownLeft;
		yield return Down;
		yield return DownRight;
		yield return Right;
		yield return UpRight;
	}
	/// <summary>
	/// Enumerates the four main compass directions.
	/// </summary>
	public static IEnumerable<Direction> FourDirections()
	{
		yield return Up;
		yield return Down;
		yield return Right;
		yield return Left;
	}

	public static IEnumerable<Direction> EightDirections()
	{
		yield return Up;
		yield return Down;
		yield return Left;
		yield return Right;
		yield return UpLeft;
		yield return UpRight;
		yield return DownLeft;
		yield return DownRight;
	}

	public static IEnumerable<Direction> DiagonalDirections()
	{
		yield return UpLeft;
		yield return UpRight;
		yield return DownLeft;
		yield return DownRight;
	}

	public Direction Rotate8Way( int times, bool clockwise = true )
	{
		Direction direction = this;
		if( times < 0 )
		{
			times = -times;
			clockwise = !clockwise;
		}

		for( int i = 0; i < times; i++ )
		{
			direction = clockwise ? direction.Previous : direction.Next;
		}
		return direction;
	}

	public Direction Rotate4Way( int times, bool clockwise = true )
	{
		Direction direction = this;
		if( times < 0 )
		{
			times = -times;
			clockwise = !clockwise;
		}

		for( int i = 0; i < times; i++ )
		{
			direction = clockwise ? direction.RotateLeft90 : direction.RotateRight90;
		}
		return direction;
	}

	public Direction Rotate( bool clockwise = true )
	{
		if( Globals.DefaultMetric == DistanceMetric.Chebyshev )
		{
			return clockwise ? Next : Previous;
		}
		else
		{
			return clockwise ? RotateRight90 : RotateLeft90;
		}
	}

	/// <summary>
	/// Gets a Direction heading from the origin towards the given position. target.pos - my.pos
	/// </summary>
	/// <param name="position"></param>
	/// <returns>Direction</returns>
	public static Direction Towards( Vec position )
	{
		int offsetX = 0;
		int offsetY = 0;

		if( position.x < 0 ) offsetX = -1;
		if( position.x > 0 ) offsetX = 1;
		if( position.y < 0 ) offsetY = -1;
		if( position.y > 0 ) offsetY = 1;

		return new Direction( (offsetX, offsetY) );
	}

	/// <summary>
	/// Gets the <see cref="Direction"/> following this one in clockwise order.
	/// Will wrap around. If this direction is None, then returns None.
	/// </summary>
	public Direction Next
	{
		get
		{
			if( this == Up ) return UpRight;
			else if( this == UpRight ) return Right;
			else if( this == Right ) return DownRight;
			else if( this == DownRight ) return Down;
			else if( this == Down ) return DownLeft;
			else if( this == DownLeft ) return Left;
			else if( this == Left ) return UpLeft;
			else if( this == UpLeft ) return Up;
			else return None;
		}
	}

	/// <summary>
	/// Gets the <see cref="Direction"/> following this one in counterclockwise
	/// order. Will wrap around. If this direction is None, then returns None.
	/// </summary>
	public Direction Previous
	{
		get
		{
			if( this == Up ) return UpLeft;
			else if( this == UpRight ) return Up;
			else if( this == Right ) return UpRight;
			else if( this == DownRight ) return Right;
			else if( this == Down ) return DownRight;
			else if( this == DownLeft ) return Down;
			else if( this == Left ) return DownLeft;
			else if( this == UpLeft ) return Left;
			else return None;
		}
	}

	public Direction RotateLeft90 => Previous.Previous;

	public Direction RotateRight90 => Next.Next;

	public Direction Rotate180 => new Direction( offset * -1 );

	public Direction RandomDirection() => Globals.DefaultMetric == DistanceMetric.Manhattan ? FourDirections().GetRandom() : EightDirections().GetRandom();

	#region IEquatable<Direction> Members

	public override bool Equals( object obj )
	{
		// check the type
		if( obj == null ) return false;
		if( !( obj is Direction ) ) return false;

		// call the typed Equals
		return Equals( (Direction)obj );
	}

	public override int GetHashCode() => Offset.GetHashCode();

	public override string ToString()
	{
		if( this == Up ) return "N";
		else if( this == UpRight ) return "NE";
		else if( this == Right ) return "E";
		else if( this == DownRight ) return "SE";
		else if( this == Down ) return "S";
		else if( this == DownLeft ) return "SW";
		else if( this == Left ) return "W";
		else if( this == UpLeft ) return "NW";
		else if( this == None ) return "None";

		return Offset.ToString();
	}

	public bool Equals( Direction other ) => Offset.Equals( other.Offset );

	#endregion IEquatable<Direction> Members

	public static implicit operator int( Direction value )
	{
		if( value == Up ) return 8;
		else if( value == UpRight ) return 9;
		else if( value == Right ) return 6;
		else if( value == DownRight ) return 3;
		else if( value == Down ) return 2;
		else if( value == DownLeft ) return 1;
		else if( value == Left ) return 4;
		else if( value == UpLeft ) return 7;
		else return 0;
	}

	public static implicit operator Direction( int value )
	{
		if( value == 8 ) return Up;
		else if( value == 9 ) return UpRight;
		else if( value == 6 ) return Right;
		else if( value == 3 ) return DownRight;
		else if( value == 2 ) return Down;
		else if( value == 1 ) return DownLeft;
		else if( value == 4 ) return Left;
		else if( value == 7 ) return UpLeft;
		else return None;
	}
}