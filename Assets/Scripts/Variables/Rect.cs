// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.

using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// A 2D integer rectangle class. Similar to Rectangle, but not dependent on System.Drawing
/// and much more feature-rich.
/// </summary>
[Serializable]
public struct Rect : IEquatable<Rect>, IEnumerable<Vec>, IComparable<Rect>, IComparable
{
	private readonly Vec position;

	private readonly Vec size;

	/// <summary>
	/// Gets the empty rectangle.
	/// </summary>
	public static readonly Rect Empty;

	/// <summary>
	/// Creates a new rectangle a single row in height, as wide as the given size.
	/// </summary>
	/// <param name="size">The width of the rectangle.</param>
	/// <returns>The new rectangle.</returns>
	public static Rect Row( int size ) => new Rect( 0, 0, size, 1 );

	/// <summary>
	/// Creates a new rectangle a single row in height, as wide as the given size,
	/// starting at the given top-left corner.
	/// </summary>
	/// <param name="x">The left edge of the rectangle.</param>
	/// <param name="y">The top of the rectangle.</param>
	/// <param name="size">The width of the rectangle.</param>
	/// <returns>The new rectangle.</returns>
	public static Rect Row( int x, int y, int size ) => new Rect( x, y, size, 1 );

	/// <summary>
	/// Creates a new rectangle a single row in height, as wide as the given size,
	/// starting at the given top-left corner.
	/// </summary>
	/// <param name="pos">The top-left corner of the rectangle.</param>
	/// <param name = "size"></param>
	/// <returns>The new rectangle.</returns>
	public static Rect Row( Vec pos, int size ) => new Rect( pos.x, pos.y, size, 1 );

	/// <summary>
	/// Creates a new rectangle a single column in width, as tall as the given size.
	/// </summary>
	/// <param name="size">The height of the rectangle.</param>
	/// <returns>The new rectangle.</returns>
	public static Rect Column( int size ) => new Rect( 0, 0, 1, size );

	/// <summary>
	/// Creates a new rectangle a single column in width, as tall as the given size,
	/// starting at the given top-left corner.
	/// </summary>
	/// <param name="x">The left edge of the rectangle.</param>
	/// <param name="y">The top of the rectangle.</param>
	/// <param name="size">The height of the rectangle.</param>
	/// <returns>The new rectangle.</returns>
	public static Rect Column( int x, int y, int size ) => new Rect( x, y, 1, size );

	/// <summary>
	/// Creates a new rectangle a single column in width, as tall as the given size,
	/// starting at the given top-left corner.
	/// </summary>
	/// <param name="pos">The top-left corner of the rectangle.</param>
	/// <param name="size">The height of the rectangle.</param>
	/// <returns>The new rectangle.</returns>
	public static Rect Column( Vec pos, int size ) => new Rect( pos.x, pos.y, 1, size );

	/// <summary>
	/// Creates a new rectangle that is the intersection of the two given rectangles.
	/// </summary>
	/// <example><code>
	/// .----------.
	/// | a        |
	/// | .--------+----.
	/// | | result |  b |
	/// | |        |    |
	/// '-+--------'    |
	///   |             |
	///   '-------------'
	/// </code></example>
	/// <param name="a">The first rectangle.</param>
	/// <param name="b">The rectangle to intersect it with.</param>
	/// <returns></returns>
	public static Rect Intersect( Rect a, Rect b )
	{
		int left = Math.Max( a.Left, b.Left );
		int right = Math.Min( a.Right, b.Right );
		int top = Math.Max( a.Top, b.Top );
		int bottom = Math.Min( a.Bottom, b.Bottom );

		int width = Math.Max( 0, right - left );
		int height = Math.Max( 0, bottom - top );

		return new Rect( left, top, width, height );
	}

	public static Rect CenterIn( Rect toCenter, Rect main )
	{
		Vec pos = main.Position + ( ( main.Size - toCenter.Size ) / 2 );

		return new Rect( pos, toCenter.Size );
	}

	#region Operators

	public static bool operator ==( Rect r1, Rect r2 )
	{
		return r1.Equals( r2 );
	}

	public static bool operator !=( Rect r1, Rect r2 )
	{
		return !r1.Equals( r2 );
	}

	public static Rect operator +( Rect r1, Vec v2 )
	{
		return new Rect( r1.Position + v2, r1.Size );
	}

	public static Rect operator +( Vec v1, Rect r2 )
	{
		return new Rect( r2.Position + v1, r2.Size );
	}

	public static Rect operator -( Rect r1, Vec v2 )
	{
		return new Rect( r1.Position - v2, r1.Size );
	}

	#endregion Operators

	public Vec Position => position;
	public Vec Size => size;

	// ReSharper disable once InconsistentNaming
	public int x => position.x;

	// ReSharper disable once InconsistentNaming
	public int y => position.y;
	public int Width => size.x;
	public int Height => size.y;

	public int Left => x;
	public int Top => y;
	public int Right => x + Width;
	public int Bottom => y + Height;

	public Vec TopLeft => new Vec( Left, Top );
	public Vec TopRight => new Vec( Right, Top );
	public Vec BottomLeft => new Vec( Left, Bottom );
	public Vec BottomRight => new Vec( Right, Bottom );

	public Vec Center => new Vec( ( Left + Right ) / 2, ( Top + Bottom ) / 2 );

	public int Area => size.Area;

	public Rect( Vec pos, Vec size )
	{
		position = pos;
		this.size = size;
	}

	public Rect( Vec size )
		: this( Vec.Zero, size )
	{
	}

	public Rect( int x, int y, int width, int height )
		: this( new Vec( x, y ), new Vec( width, height ) )
	{
	}

	public Rect( Vec pos, int width, int height )
		: this( pos, new Vec( width, height ) )
	{
	}

	public Rect( int width, int height )
		: this( new Vec( width, height ) )
	{
	}

	public Rect( int x, int y, Vec size )
		: this( new Vec( x, y ), size )
	{
	}

	static Rect()
	{
		Empty = new Rect( Vec.Zero, Vec.Zero );
	}


	public override string ToString() => $"Rect:{position.ToString()}.{size.ToString()}";

	#region Comparer

	/// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object. </summary>
	/// <param name="other">An object to compare with this instance. </param>
	/// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order. </returns>
	public int CompareTo( Rect other )
	{
		int positionComparison = position.CompareTo( other.position );

		if ( positionComparison != 0 ) return positionComparison;

		return size.CompareTo( other.size );
	}

	/// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
	/// <param name="obj">An object to compare with this instance. </param>
	/// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order. </returns>
	/// <exception cref="T:System.ArgumentException">
	/// <paramref name="obj" /> is not the same type as this instance. </exception>
	public int CompareTo( object obj )
	{
		if ( ReferenceEquals( null, obj ) ) return 1;

		return obj is Rect other ? CompareTo( other ) : throw new ArgumentException( $"Object must be of type {nameof(Rect)}" );
	}

	/// <summary>Returns a value that indicates whether a <see cref="T:Rect" /> value is less than another <see cref="T:Rect" /> value.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>true if <paramref name="left" /> is less than <paramref name="right" />; otherwise, false.</returns>
	public static bool operator <( Rect left, Rect right )
	{
		return left.CompareTo( right ) < 0;
	}

	/// <summary>Returns a value that indicates whether a <see cref="T:Rect" /> value is greater than another <see cref="T:Rect" /> value.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>true if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, false.</returns>
	public static bool operator >( Rect left, Rect right )
	{
		return left.CompareTo( right ) > 0;
	}

	/// <summary>Returns a value that indicates whether a <see cref="T:Rect" /> value is less than or equal to another <see cref="T:Rect" /> value.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>true if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, false.</returns>
	public static bool operator <=( Rect left, Rect right )
	{
		return left.CompareTo( right ) <= 0;
	}

	/// <summary>Returns a value that indicates whether a <see cref="T:Rect" /> value is greater than or equal to another <see cref="T:Rect" /> value.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>true if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, false.</returns>
	public static bool operator >=( Rect left, Rect right )
	{
		return left.CompareTo( right ) >= 0;
	}

	#endregion

	/// <summary>Indicates whether this instance and a specified object are equal.</summary>
	/// <param name="obj">The object to compare with the current instance. </param>
	/// <returns>
	/// <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />. </returns>
	public override bool Equals( object obj )
	{
		return obj is Rect other && Equals( other );
	}

	/// <summary>Returns the hash code for this instance.</summary>
	/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
	public override int GetHashCode()
	{
		unchecked
		{
			return ( position.GetHashCode() * 397 ) ^ size.GetHashCode();
		}
	}

	public Rect Offset( Vec pos, Vec size ) => new Rect( this.position + pos, this.size + size );

	public Rect Offset( int x, int y, int width, int height ) => Offset( new Vec( x, y ), new Vec( width, height ) );

	[SuppressMessage( "ReSharper", "PossiblyImpureMethodCallOnReadonlyVariable" )]
	public Rect Inflate( int distance ) => new Rect( position.Offset( -distance, -distance ), size.Offset( distance * 2, distance * 2 ) );

	public bool Contains( Vec pos )
	{
		if ( pos.x < position.x ) return false;
		if ( pos.x >= position.x + size.x ) return false;
		if ( pos.y < position.y ) return false;
		if ( pos.y >= position.y + size.y ) return false;

		return true;
	}

	public bool Contains( Rect rect )
	{
		// all sides must be within
		if ( rect.Left < Left ) return false;
		if ( rect.Right > Right ) return false;
		if ( rect.Top < Top ) return false;
		if ( rect.Bottom > Bottom ) return false;

		return true;
	}

	public bool Overlaps( Rect rect )
	{
		// fail if they do not overlap on any axis
		if ( Left > rect.Right ) return false;
		if ( Right < rect.Left ) return false;
		if ( Top > rect.Bottom ) return false;
		if ( Bottom < rect.Top ) return false;

		// then they must overlap
		return true;
	}

	public Rect Intersect( Rect rect ) => Intersect( this, rect );

	public Rect CenterIn( Rect rect ) => CenterIn( this, rect );

	public IEnumerable<Vec> Trace()
	{
		if ( ( Width > 1 ) && ( Height > 1 ) )
		{
			// trace all four sides
			foreach ( Vec top in Row( TopLeft, Width - 1 ) ) yield return top;
			foreach ( Vec right in Column( TopRight.OffsetX( -1 ), Height - 1 ) ) yield return right;
			foreach ( Vec bottom in Row( Width - 1 ) ) yield return BottomRight.Offset( -1, -1 ) - bottom;
			foreach ( Vec left in Column( Height - 1 ) ) yield return BottomLeft.OffsetY( -1 ) - left;
		}
		else if ( ( Width > 1 ) && ( Height == 1 ) )
		{
			// a single row
			foreach ( Vec pos in Row( TopLeft, Width ) ) yield return pos;
		}
		else if ( ( Height >= 1 ) && ( Width == 1 ) )
		{
			// a single column, or one unit
			foreach ( Vec pos in Column( TopLeft, Height ) ) yield return pos;
		}

		// otherwise, the rect doesn't have a positive size, so there's nothing to trace
	}

	#region IEquatable<Rect> Members

	public bool Equals( Rect other )
	{
		return position.Equals( other.position ) && size.Equals( other.size );
	}

	#endregion IEquatable<Rect> Members

	#region IEnumerable<Vec> Members

	public IEnumerator<Vec> GetEnumerator()
	{
		if ( size.x < 0 ) throw new ArgumentOutOfRangeException( $"Cannot enumerate a Rectangle with a negative width." );
		if ( size.y < 0 ) throw new ArgumentOutOfRangeException( $"Cannot enumerate a Rectangle with a negative height." );

		for ( int y = position.y; y < position.y + size.y; y++ )

		for ( int x = position.x; x < position.x + size.x; x++ )
		{
			yield return new Vec( x, y );
		}
	}

	#endregion IEnumerable<Vec> Members

	#region IEnumerable Members

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	#endregion IEnumerable Members
}