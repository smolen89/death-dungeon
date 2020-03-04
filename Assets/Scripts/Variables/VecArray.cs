// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class VecArray<T>
{
	[SerializeField]    private int width;
	[SerializeField]    private int height;
	[SerializeField]    private T[] array;

	public T[] Get() => array;

	/// <summary>
	/// Gets the width of the array.
	/// </summary>
	public int Width => width;

	/// <summary>
	/// Gets the height of the array.
	/// </summary>
	public int Height => height;

	/// <summary>
	/// Initializes a new instance of Array2D with the given dimensions.
	/// </summary>
	/// <param name="width">Width of the array.</param>
	/// <param name="height">Height of the array.</param>
	public VecArray( int width, int height )
	{
		if( width < 0 )
		{
			Debugger.LogError( "VecArray", $"Width must be greater than zero." );
			return;
		}
		if( height < 0 )
		{
			Debugger.LogError( "VecArray", $"Height must be greater than zero. " );
			return;
		}

		this.width = width;
		this.height = height;

		array = new T[ width * height ];
		Size = new Vec( width, height );
		Bounds = new Rect( width, height );
	}

	/// <summary>
	/// Initializes a new instance of Array2D with the given size.
	/// </summary>
	/// <param name="size">Size of the array.</param>
	public VecArray( Vec size ) : this( size.x, size.y )
	{
	}

	/// <summary>
	/// Gets the size of the array.
	/// </summary>
	public readonly Vec Size;

	/// <summary>
	/// Gets the bounds of the array. The top-level coordinate will be the origin.
	/// </summary>
	public readonly Rect Bounds;

	public int Length => width * height;

	/// <summary>
	/// Gets and sets the array element at the given position.
	/// </summary>
	/// <param name="x">The X-coordinate of the element.</param>
	/// <param name="y">The Y-coordinate of the element.</param>
	/// <exception cref="IndexOutOfRangeException">if the position is out of bounds.</exception>
	public T this[ int x, int y ]
	{
		get => array[ y * width + x ];
		set => array[ y * width + x ] = value;
	}

	/// <summary>
	/// Gets and sets the array element at the given position.
	/// </summary>
	/// <param name="position">The position of the element. Must be within bounds.</param>
	/// <exception cref="IndexOutOfRangeException">if the position is out of bounds.</exception>
	public T this[ Vec position ]
	{
		get => array[ position.y * width + position.x ];
		set => array[ position.y * width + position.x ] = value;
	}

	/// <summary>
	/// Fills all of the elements in the array with the given value.
	/// </summary>
	/// <param name="value">The value to fill the array with.</param>
	public void Fill( T value )
	{
		foreach( Vec pos in Bounds )
		{
			this[ pos ] = value;
		}
	}

	/// <summary>
	/// Fills all of the elements in the array with values returned by the given callback.
	/// </summary>
	/// <param name="callback">The function to call for each element in the array.</param>
	public void Fill( Func<Vec, T> callback )
	{
		foreach( Vec pos in Bounds )
		{
			this[ pos ] = callback( pos );
		}
	}

	public bool BoundsCheck( Vec position, bool allowMapEdges = true ) => BoundsCheck( position.x, position.y, allowMapEdges );

	public bool BoundsCheck( int x, int y, bool allowMapEdges = true )
	{
		if( x >= 0 && x < width && y >= 0 && y < height )
		{
			if( !allowMapEdges )
			{
				if( IsOnBorder( x, y ) )
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public bool IsOnBorder( Vec position ) => IsOnBorder( position.x, position.y );

	public bool IsOnBorder( int x, int y ) => x == 0 || x == width - 1 || y == 0 || y == height - 1;

	public void Foreach( Action<Vec> action )
	{
		foreach( Vec pos in Bounds )
		{
			action?.Invoke( pos );
		}
	}

	public IEnumerable<T> GetAll()
	{
		for( int x = 0; x < width; x++ )
		{
			for( int y = 0; y < height; y++ )
			{
				yield return array[ y * width + x ];
			}
		}
	}

	public IEnumerator GetEnumerator() => array.GetEnumerator();

	public override string ToString() => $"VecArray<{typeof( T )}> [{width.ToString()}, {height.ToString()}]";
}