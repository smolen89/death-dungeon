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
	public static Direction None => new Direction( Vec.Zero );

	public static Direction N => new Direction( new Vec( 0, 1 ) );
	public static Direction NE => new Direction( new Vec( 1, 1 ) );
	public static Direction E => new Direction( new Vec( 1, 0 ) );
	public static Direction SE => new Direction( new Vec( 1, -1 ) );
	public static Direction S => new Direction( new Vec( 0, -1 ) );
	public static Direction SW => new Direction( new Vec( -1, -1 ) );
	public static Direction W => new Direction( new Vec( -1, 0 ) );
	public static Direction NW => new Direction( new Vec( -1, 1 ) );

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
	/// Enumerates the directions in clockwise order, starting with <see cref="N"/>.
	/// </summary>
	public static IList<Direction> Clockwise => new List<Direction> { N, NE, E, SE, S, SW, W, NW };

	/// <summary>
	/// Enumerates the directions in counterclockwise order, starting with <see cref="N"/>.
	/// </summary>
	public static IList<Direction> Counterclockwise => new List<Direction> { N, NW, W, SW, S, SE, E, NE };

	/// <summary>
	/// Enumerates the four main compass directions.
	/// </summary>
	public static IList<Direction> FourDirections => new List<Direction> { N, S, E, W };

	public static IList<Direction> EightDirections => new List<Direction> { N, NE, E, SE, S, SW, W, NW };

	public static IList<Direction> DiagonalDirections => new List<Direction> { NE, SE, SW, NW };

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

	public Direction Rotate(bool clockwise = true )
	{
		if (Globals.DefaultMetric == DistanceMetric.Chebyshev )
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
		Vec offset = Vec.Zero;

		if( position.x < 0 ) offset.x = -1;
		if( position.x > 0 ) offset.x = 1;
		if( position.y < 0 ) offset.y = -1;
		if( position.y > 0 ) offset.y = 1;

		return new Direction( offset );
	}

	/// <summary>
	/// Gets the <see cref="Direction"/> following this one in clockwise order.
	/// Will wrap around. If this direction is None, then returns None.
	/// </summary>
	public Direction Next
	{
		get
		{
			if( this == N ) return NE;
			else if( this == NE ) return E;
			else if( this == E ) return SE;
			else if( this == SE ) return S;
			else if( this == S ) return SW;
			else if( this == SW ) return W;
			else if( this == W ) return NW;
			else if( this == NW ) return N;
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
			if( this == N ) return NW;
			else if( this == NE ) return N;
			else if( this == E ) return NE;
			else if( this == SE ) return E;
			else if( this == S ) return SE;
			else if( this == SW ) return S;
			else if( this == W ) return SW;
			else if( this == NW ) return W;
			else return None;
		}
	}

	public Direction RotateLeft90 => Previous.Previous;

	public Direction RotateRight90 => Next.Next;

	public Direction Rotate180 => new Direction( offset * -1 );

	public Direction RandomDirection() => Globals.DefaultMetric == DistanceMetric.Manhattan ? FourDirections.GetRandom() : EightDirections.GetRandom();

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
		if( this == N ) return "N";
		else if( this == NE ) return "NE";
		else if( this == E ) return "E";
		else if( this == SE ) return "SE";
		else if( this == S ) return "S";
		else if( this == SW ) return "SW";
		else if( this == W ) return "W";
		else if( this == NW ) return "NW";
		else if( this == None ) return "None";

		return Offset.ToString();
	}

	public bool Equals( Direction other ) => Offset.Equals( other.Offset );

	#endregion IEquatable<Direction> Members

	public static implicit operator int(Direction value )
	{
		if( value == N ) return 8;
		else if( value == NE ) return 9;
		else if( value == E ) return 6;
		else if( value == SE ) return 3;
		else if( value == S ) return 2;
		else if( value == SW ) return 1;
		else if( value == W ) return 4;
		else if( value == NW ) return 7;
		else return 0;
	}

	public static implicit operator Direction (int value )
	{
		if( value == 8 ) return N;
		else if( value == 9 ) return NE;
		else if( value == 6 ) return E;
		else if( value == 3 ) return SE;
		else if( value == 2 ) return S;
		else if( value == 1 ) return SW;
		else if( value == 4 ) return W;
		else if( value == 7 ) return NW;
		else return None;
	}
}