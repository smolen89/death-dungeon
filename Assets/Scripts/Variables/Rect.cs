// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A 2D integer rectangle class. Similar to Rectangle, but not dependent on System.Drawing
/// and much more feature-rich.
/// </summary>
[Serializable]
public struct Rect : IEquatable<Rect>, IEnumerable<Vec>
{
	[SerializeField]
	private Vec position;

	[SerializeField]
	private Vec size;

	/// <summary>
	/// Gets the empty rectangle.
	/// </summary>
	public readonly static Rect Empty;

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
		int left = Math.Max(a.Left, b.Left);
		int right = Math.Min(a.Right, b.Right);
		int top = Math.Max(a.Top, b.Top);
		int bottom = Math.Min(a.Bottom, b.Bottom);

		int width = Math.Max(0, right - left);
		int height = Math.Max(0, bottom - top);

		return new Rect( left, top, width, height );
	}

	public static Rect CenterIn( Rect toCenter, Rect main )
	{
		Vec pos = main.Position + ((main.Size - toCenter.Size) / 2);

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

	public Vec Position { get { return position; } }
	public Vec Size { get { return size; } }

	public int x { get { return position.x; } }
	public int y { get { return position.y; } }
	public int Width { get { return size.x; } }
	public int Height { get { return size.y; } }

	public int Left { get { return x; } }
	public int Top { get { return y; } }
	public int Right { get { return x + Width; } }
	public int Bottom { get { return y + Height; } }

	public Vec TopLeft { get { return new Vec( Left, Top ); } }
	public Vec TopRight { get { return new Vec( Right, Top ); } }
	public Vec BottomLeft { get { return new Vec( Left, Bottom ); } }
	public Vec BottomRight { get { return new Vec( Right, Bottom ); } }

	public Vec Center { get { return new Vec( ( Left + Right ) / 2, ( Top + Bottom ) / 2 ); } }

	public int Area { get { return size.Area; } }

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

	public override string ToString() => $"({position})-({size})";

	public override bool Equals( object obj )
	{
		if( obj is Rect ) return Equals( (Rect)obj );

		return base.Equals( obj );
	}

	public override int GetHashCode() => position.GetHashCode() + size.GetHashCode();

	public Rect Offset( Vec pos, Vec size ) => new Rect( this.position + pos, this.size + size );

	public Rect Offset( int x, int y, int width, int height ) => Offset( new Vec( x, y ), new Vec( width, height ) );

	public Rect Inflate( int distance ) => new Rect( position.Offset( -distance, -distance ), size.Offset( distance * 2, distance * 2 ) );

	public bool Contains( Vec pos )
	{
		if( pos.x < position.x ) return false;
		if( pos.x >= position.x + size.x ) return false;
		if( pos.y < position.y ) return false;
		if( pos.y >= position.y + size.y ) return false;

		return true;
	}

	public bool Contains( Rect rect )
	{
		// all sides must be within
		if( rect.Left < Left ) return false;
		if( rect.Right > Right ) return false;
		if( rect.Top < Top ) return false;
		if( rect.Bottom > Bottom ) return false;

		return true;
	}

	public bool Overlaps( Rect rect )
	{
		// fail if they do not overlap on any axis
		if( Left > rect.Right ) return false;
		if( Right < rect.Left ) return false;
		if( Top > rect.Bottom ) return false;
		if( Bottom < rect.Top ) return false;

		// then they must overlap
		return true;
	}

	public Rect Intersect( Rect rect ) => Intersect( this, rect );

	public Rect CenterIn( Rect rect ) => CenterIn( this, rect );

	public IEnumerable<Vec> Trace()
	{
		if( ( Width > 1 ) && ( Height > 1 ) )
		{
			// trace all four sides
			foreach( Vec top in Row( TopLeft, Width - 1 ) ) yield return top;
			foreach( Vec right in Column( TopRight.OffsetX( -1 ), Height - 1 ) ) yield return right;
			foreach( Vec bottom in Row( Width - 1 ) ) yield return BottomRight.Offset( -1, -1 ) - bottom;
			foreach( Vec left in Column( Height - 1 ) ) yield return BottomLeft.OffsetY( -1 ) - left;
		}
		else if( ( Width > 1 ) && ( Height == 1 ) )
		{
			// a single row
			foreach( Vec pos in Row( TopLeft, Width ) ) yield return pos;
		}
		else if( ( Height >= 1 ) && ( Width == 1 ) )
		{
			// a single column, or one unit
			foreach( Vec pos in Column( TopLeft, Height ) ) yield return pos;
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
		if( size.x < 0 ) throw new ArgumentOutOfRangeException( "Cannot enumerate a Rectangle with a negative width." );
		if( size.y < 0 ) throw new ArgumentOutOfRangeException( "Cannot enumerate a Rectangle with a negative height." );

		for( int y = position.y; y < position.y + size.y; y++ )
		{
			for( int x = position.x; x < position.x + size.x; x++ )
			{
				yield return new Vec( x, y );
			}
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