// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System;
using System.Collections.Generic;

public static class VecArrayExtensions
{
	public static List<Vec> AllPositions<T>( this VecArray<T> array )
	{
		List<Vec> result = new List<Vec>();
		array.Foreach( pos => result.Add( pos ) );
		//int width = array.Width;
		//int height = array.Height;
		//for( int x = 0; x < width; ++x )
		//{
		//	for( int y = 0; y < height; ++y )
		//	{
		//		result.Add( new Vec( x, y ) );
		//	}
		//}
		return result;
	}

	public static Vec RandomPosition<T>( this VecArray<T> array, bool allow_borders )
	{
		if( allow_borders )
		{
			//return new Vec( new Rand().RangeInclusive( 0, array.Width - 1 ), new Rand().RangeInclusive( 0, array.Height - 1 ) );
			int x = UnityEngine.Random.Range(0,array.Width-1);
			int y = UnityEngine.Random.Range(0,array.Height-1);
			return new Vec( x, y );
		}
		else
		{
			//return new Vec( new Rand().RangeInclusive( 1, array.Width - 2 ), new Rand().RangeInclusive( 1, array.Height - 2 ) );
			int x = UnityEngine.Random.Range(1,array.Width-2);
			int y = UnityEngine.Random.Range(1,array.Height-2);
			return new Vec( x, y );
		}
	}

	public static List<Vec> PositionsWhere<T>( this VecArray<T> array, BooleanPositionDelegate condition )
	{
		List<Vec> result = new List<Vec>();
		for( int x = 0; x < array.Width; ++x )
		{
			for( int y = 0; y < array.Height; ++y )
			{
				Vec p = new Vec( x, y );
				if( condition( p ) )
				{
					result.Add( p );
				}
			}
		}
		return result;
	}

	public static List<Vec> PositionsWhereGreatest<T>( this VecArray<T> array, IntegerPositionDelegate value )
	{
		List<Vec> result = new List<Vec>();
		int highest = 0;
		bool first = true;

		for( int x = 0; x < array.Width; ++x )
		{
			for( int y = 0; y < array.Height; ++y )
			{
				Vec position = new Vec(x,y);
				int newValue = value(position);
				if( first )
				{
					first = false;
					highest = newValue;
					result.Add( position );
				}
				else
				{
					if( newValue > highest )
					{
						highest = newValue;
						result.Clear();
						result.Add( position );
					}
					else
					{
						if( newValue == highest )
						{
							result.Add( position );
						}
					}
				}
			}
		}
		return result;
	}

	public static List<Vec> PositionsWhereLeast<T>( this VecArray<T> array, IntegerPositionDelegate value )
	{
		List<Vec> result = new List<Vec>();
		int lowest = 0;
		bool first = true;
		for( int x = 0; x < array.Width; ++x )
		{
			for( int y = 0; y < array.Height; ++y )
			{
				Vec position = new Vec(x,y);
				int newValue = value(position);
				if( first )
				{
					first = false;
					lowest = newValue;
					result.Add( position );
				}
				else
				{
					if( newValue < lowest )
					{
						lowest = newValue;
						result.Clear();
						result.Add( position );
					}
					else
					{
						if( newValue == lowest )
						{
							result.Add( position );
						}
					}
				}
			}
		}
		return result;
	}

	public static List<Vec> PositionsNearBorder<T>( this VecArray<T> array, int distance )
	{
		List<Vec> result = new List<Vec>();
		if( distance <= 0 )
		{
			return result;
		}
		int Width = array.Get().GetLength(0);
		int Height = array.Get().GetLength(1);
		if( distance * 2 >= Width && distance * 2 >= Height )
		{
			return array.AllPositions();
		}
		for( int x = 0; x < Width; ++x )
		{
			for( int y = 0; y < Height; ++y )
			{
				if( y == distance && x >= distance && x < Width - distance )
				{
					y = Height - distance;
				}
				result.Add( new Vec( x, y ) );
			}
		}
		return result;
	}

	public static List<Vec> GetFloodFillPositions<T>( this VecArray<T> array, Vec origin, bool exclude_origin, BooleanPositionDelegate condition )
		=> array.GetFloodFillPositions( new List<Vec> { origin }, exclude_origin, condition );

	public static List<Vec> GetFloodFillPositions<T>( this VecArray<T> array, List<Vec> origins, bool exclude_origins, BooleanPositionDelegate condition )
	{
		List<Vec> result = new List<Vec>( origins );
		VecArray<bool> result_map = new VecArray<bool>( array.Width, array.Height );

		foreach( Vec origin in origins )
		{
			result_map[origin] = true;
		}

		Queue<Vec> frontier = new Queue<Vec>( origins );

		while( frontier.Count > 0 )
		{
			Vec position = frontier.Dequeue();
			foreach( Vec neighbor in position.PositionsAtDistance( 1 ) )
			{
				if( array.BoundsCheck(neighbor) && !result_map[neighbor] && condition( neighbor ) )
				{
					result_map[neighbor] = true;
					frontier.Enqueue( neighbor );
					result.Add( neighbor );
				}
			}
		}
		if( exclude_origins )
		{
			foreach( Vec origin in origins )
			{
				result.Remove( origin );
			}
		}
		return result;
	}

	public static VecArray<bool> GetFloodFillArray<T>( this VecArray<T> array, Vec origin, bool exclude_origin, BooleanPositionDelegate condition )
		=> array.GetFloodFillArray( new List<Vec> { origin }, exclude_origin, condition );

	public static VecArray<bool> GetFloodFillArray<T>( this VecArray<T> array, List<Vec> origins, bool exclude_origins, BooleanPositionDelegate condition )
	{
		VecArray<bool> result_map = new VecArray<bool>( array.Width, array.Height );
		foreach( Vec origin in origins )
		{
			result_map[origin] = true;
		}
		Queue<Vec> frontier = new Queue<Vec>(origins);
		while( frontier.Count > 0 )
		{
			Vec p = frontier.Dequeue();
			foreach( Vec neighbor in p.PositionsAtDistance( 1 ) )
			{
				if( array.BoundsCheck(neighbor) && !result_map[neighbor] && condition( neighbor ) )
				{
					result_map[neighbor] = true;
					frontier.Enqueue( neighbor );
				}
			}
		}
		if( exclude_origins )
		{
			foreach( Vec origin in origins )
			{
				result_map[origin] = false;
			}
		}
		return result_map;
	}

	public static List<Vec> GetRandomizedFloodFillPositions<T>(
		this VecArray<T> array, Vec origin, int desired_count, bool exclude_origin_from_count, bool exclude_origin_from_result, BooleanPositionDelegate condition )
		=> array.GetRandomizedFloodFillPositions( new List<Vec> { origin }, desired_count, exclude_origin_from_count, exclude_origin_from_result, condition );

	public static List<Vec> GetRandomizedFloodFillPositions<T>(
		this VecArray<T> array,		List<Vec> origins,		int desired_count,		bool exclude_origins_from_count,		bool exclude_origins_from_result,		BooleanPositionDelegate condition )
	{
		List<Vec> result = new List<Vec>();
		VecArray<bool> result_map = new VecArray<bool>( array.Width, array.Height );
		List<Vec> frontier = new List<Vec>();
		int count = 0;

		foreach( Vec origin in origins )
		{
			result_map[origin] = true;
			if( condition( origin ) )
			{
				if( !exclude_origins_from_count )
				{
					++count;
				}
				if( !exclude_origins_from_result )
				{
					result.Add( origin );
				}
			}
			foreach( Vec neighbor in origin.PositionsAtDistance( 1 ) )
			{
				if( array.BoundsCheck(neighbor) && !result_map[neighbor] )
				{
					result_map[neighbor] = true;
					frontier.Add( neighbor );
				}
			}
		}
		while( frontier.Count > 0 && count < desired_count )
		{
			int randIndex = new Rand().Range( 0, frontier.Count );
			Vec p = frontier[randIndex];
			frontier.RemoveAt( randIndex );
			if( condition( p ) )
			{
				result.Add( p );
				++count;
				foreach( Vec neighbor in p.PositionsAtDistance( 1 ) )
				{
					if(array.BoundsCheck(neighbor) && !result_map[neighbor] )
					{
						result_map[neighbor] = true;
						frontier.Add( neighbor );
					}
				}
			}
		}
		return result;
	}
}