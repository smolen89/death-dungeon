// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System;
using UnityEngine;

/// <summary>
/// A 2D integer vector class. Similar to Point but not dependent on System.Drawing and much more
/// feature-rich.
/// </summary>
[Serializable]
public struct Vec : IEquatable<Vec>, IComparable<Vec>
{
	/// <summary>
	/// Koordynata X (Szerokość)
	/// </summary>
	public int x;

	/// <summary>
	/// Koordynata Y (Wysokość)
	/// </summary>
	public int y;

	#region Constructors

	/// <summary>
	/// Initializes a new instance of Vec with the given coordinates.
	/// </summary>
	/// <param name="x">X coordinate.</param>
	/// <param name="y">Y coordinate.</param>
	public Vec( int x = 0, int y = 0 )
	{
		this.x = x;
		this.y = y;
	}

	/// <summary>
	/// Initializes a new instance of Vec with the given coordinates.
	/// </summary>
	/// <param name="size">X and Y coordinate.</param>
	public Vec( int size = 0 ) : this( size, size )
	{
	}

	/// <summary>
	/// Initializes a new instance of Vec with the given coordinates.
	/// </summary>
	/// <param name="x">X coordinate.</param>
	/// <param name="y">Y coordinate.</param>
	public Vec( float x, float y ) : this( (int)x, (int)y )
	{
	}

	/// <summary>
	/// Initializes a new instance of Vec
	/// </summary>
	/// <param name="other">Other vector.</param>
	public Vec( Vec other ) : this( other.x, other.y )
	{
	}

	#endregion Constructors

	#region Operators

	public static bool operator ==( Vec a, Vec b ) => a.x == b.x && a.y == b.y;
	public static bool operator !=( Vec a, Vec b ) => !( a == b );

	public static bool operator ==( Vec a, Vector2 b ) => a.x == b.x && a.y == b.y;
	public static bool operator !=( Vec a, Vector2 b ) => !( a == b );

	public static bool operator ==( Vec a, Vector2Int b ) => a.x == b.x && a.y == b.y;
	public static bool operator !=( Vec a, Vector2Int b ) => !( a == b );

	public static bool operator ==( Vec a, Vector3 b ) => a.x == b.x && a.y == b.y;
	public static bool operator !=( Vec a, Vector3 b ) => !( a == b );

	public static bool operator ==( Vec a, Vector3Int b ) => a.x == b.x && a.y == b.y;
	public static bool operator !=( Vec a, Vector3Int b ) => !( a == b );


	public static Vec operator +( Vec a, Vec b ) => new Vec( a.x + b.x, a.y + b.y );
	public static Vec operator -( Vec a, Vec b ) => new Vec( a.x - b.x, a.y - b.y );

	public static Vec operator +( Vec v1, int i2 ) => new Vec( v1.x + i2, v1.y + i2 );
	public static Vec operator -( Vec v1, int i2 ) => new Vec( v1.x - i2, v1.y - i2 );
	public static Vec operator *( Vec a, int b ) => new Vec( a.x * b, a.y * b );
	public static Vec operator /( Vec a, int b ) => new Vec( a.x / b, a.y / b );

	public static Vec operator +( int i1, Vec v2 ) => new Vec( i1 + v2.x, i1 + v2.y );
	public static Vec operator -( int i1, Vec v2 ) => new Vec( i1 - v2.x, i1 - v2.y );
	public static Vec operator *( int i1, Vec v2 ) => new Vec( i1 * v2.x, i1 * v2.y );

	public static Vector2 operator +( Vector2 a, Vec b ) => new Vector2( a.x + b.x, a.y + b.y );
	public static Vec operator +( Vec a, Vector2 b ) => new Vec( a.x + b.x, a.y + b.y );

	public static Vector2Int operator +( Vector2Int a, Vec b ) => new Vector2Int( a.x + b.x, a.y + b.y );
	public static Vec operator +( Vec a, Vector2Int b ) => new Vec( a.x + b.x, a.y + b.y );

	public static Vector3 operator +( Vector3 a, Vec b ) => new Vector3( a.x + b.x,0, a.y + b.y );
	public static Vec operator +( Vec a, Vector3 b ) => new Vec( a.x + b.x, a.y + b.y );

	public static Vector3Int operator +( Vector3Int a, Vec b ) => new Vector3Int( a.x + b.x,0, a.y + b.y );
	public static Vec operator +( Vec a, Vector3Int b ) => new Vec( a.x + b.x, a.y + b.y );

	#endregion Operators

	#region Implicit operators

	// Vector2
	public static implicit operator Vector2( Vec value ) => new Vector2( value.x, value.y );
	public static implicit operator Vec( Vector2 vec ) => new Vec( vec.x, vec.y );

	// Vector2Int
	public static implicit operator Vector2Int( Vec value ) => new Vector2Int( value.x, value.y );
	public static implicit operator Vec( Vector2Int vec ) => new Vec( vec.x, vec.y );

	// Vector3
	public static implicit operator Vector3( Vec value ) => new Vector3( value.x, 0, value.y );
	public static implicit operator Vec( Vector3 vec ) => new Vec( vec.x, vec.y );

	// Vector3Int
	public static implicit operator Vector3Int( Vec value ) => new Vector3Int( value.x, 0, value.y );
	public static implicit operator Vec( Vector3Int vec ) => new Vec( vec.x, vec.y );

	#endregion Implicit operators

	#region Functions

	/// <summary>
	/// Gets the area of a rectangle with opposite corners at (0, 0) and this Vec.
	/// </summary>
	public int Area => x * y;

	/// <summary>
	/// Gets the absolute magnitude of the Vec squared.
	/// </summary>
	public int Length => (int)Math.Sqrt( LengthSquared );

	/// <summary>
	/// Gets the absolute magnitude of the Vec.
	/// </summary>
	public int LengthSquared => ( x * x + y * y );

	/// <summary>
	/// Gets the rook length of the Vec, which is the number of squares a rook on a chessboard
	/// would need to move from (0, 0) to reach the endpoint of the Vec. Also known as
	/// Manhattan or taxicab distance.
	/// </summary>
	public int RookLength => Math.Abs( x ) + Math.Abs( y );

	/// <summary>
	/// Gets the king length of the Vec, which is the number of squares a king on a chessboard
	/// would need to move from (0, 0) to reach the endpoint of the Vec. Also known as
	/// Chebyshev distance.
	/// </summary>
	public int KingLength => Math.Max( Math.Abs( x ), Math.Abs( y ) );

	public int SqrDistance( Vec other )
	{
		int dx = x - other.x;
		int dy = y - other.y;

		return (int)Math.Sqrt( dx * dx + dy * dy );
	}

	/// <summary>
	/// Gets whether the given vector is within a rectangle
	/// from (0,0) to this vector (half-inclusive).
	/// </summary>
	public bool Contains( Vec vec )
	{
		if( vec.x < 0 ) return false;
		if( vec.x >= x ) return false;
		if( vec.y < 0 ) return false;
		if( vec.y >= y ) return false;

		return true;
	}

	public bool IsAdjacentTo( Vec other )
	{
		// not adjacent to the exact same position
		if( this == other ) return false;

		Vec offset = this - other;

		return ( Math.Abs( offset.x ) <= 1 ) && ( Math.Abs( offset.y ) <= 1 );
	}

	/// <summary>
	/// Returns a new Vec whose coordinates are the coordinates of this Vec
	/// with the given values added. This Vec is not modified.
	/// </summary>
	/// <param name="x">Distance to offset the X coordinate.</param>
	/// <param name="y">Distance to offset the Y coordinate.</param>
	/// <returns>A new Vec offset by the given coordinates.</returns>
	public Vec Offset( int x, int y ) => new Vec( this.x + x, this.y + y );

	/// <summary>
	/// Returns a new Vec whose coordinates are the coordinates of this Vec
	/// with the given value added to the X coordinate. This Vec is not modified.
	/// </summary>
	/// <param name="offset">Distance to offset the X coordinate.</param>
	/// <returns>A new Vec offset by the given X coordinate.</returns>
	public Vec OffsetX( int offset ) => new Vec( x + offset, y );

	/// <summary>
	/// Returns a new Vec whose coordinates are the coordinates of this Vec
	/// with the given value added to the Y coordinate. This Vec is not modified.
	/// </summary>
	/// <param name="offset">Distance to offset the Y coordinate.</param>
	/// <returns>A new Vec offset by the given Y coordinate.</returns>
	public Vec OffsetY( int offset ) => new Vec( x, y + offset );

	#endregion Functions

	#region Static Functions

	/// <summary>
	/// Pobiera wektor 1x1
	/// </summary>
	public static readonly Vec One = new Vec( 1, 1 );

	/// <summary>
	/// Pobiera wektor zerowy
	/// </summary>
	public static readonly Vec Zero = new Vec( 0, 0 );

	/// <summary>
	/// Zwraca najmniejsze wartości x i y z dwóch Vec.
	/// </summary>
	public static Vec Min( Vec a, Vec b ) => new Vec( Math.Min( a.x, b.x ), Math.Min( a.y, b.y ) );

	/// <summary>
	/// Zwraca największe wartości x i y z dwóch Vec.
	/// </summary>
	public static Vec Max( Vec a, Vec b ) => new Vec( Math.Max( a.x, b.x ), Math.Max( a.y, b.y ) );

	#endregion Static Functions

	#region IComparable<Vec2>, IEquatable<Vec2>

	public int CompareTo( object obj )
	{
		Vec other = (Vec)obj;

		if( x == other.x )
		{
			return y.CompareTo( other.y );
		}
		else
		{
			return x.CompareTo( other.x );
		}
	}

	public int CompareTo( Vec other )
	{
		if( x == other.x )
		{
			return y.CompareTo( other.y );
		}
		else
		{
			return x.CompareTo( other.x );
		}
	}

	public bool Equals( Vec other ) => other == this;

	public override bool Equals( object obj )
	{
		if( obj is Vec )
		{
			return (Vec)obj == this;
		}

		return false;
	}

	#endregion IComparable<Vec2>, IEquatable<Vec2>

	public override int GetHashCode() => ( ( x.GetHashCode() ^ ( y.GetHashCode() << 1 ) ) >> 1 );

	public override string ToString() => $"[{x}x{y}]";
}