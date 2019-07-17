// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System.Collections.Generic;

public class VecComparer : IEqualityComparer<Vec>
{
	public bool Equals( Vec a, Vec b ) => a == b;

	public int GetHashCode( Vec obj ) => ( ( IntegerHash( obj.x ) ^ ( IntegerHash( obj.y ) << 1 ) ) >> 1 );

	//hack sprawdzić czy nie szybszy hash będzie ze stringów (x, y)
	private int IntegerHash( int value )
	{
		// fmix32 from murmurhash
		uint h = (uint)value;
		h ^= h >> 16;
		h *= 0x85ebca6bU;
		h ^= h >> 13;
		h *= 0xc2b2ae35U;
		h ^= h >> 16;
		return (int)h;
	}
}