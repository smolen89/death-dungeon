// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.

using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class ListExtensions
{
	public static T GetRandom<T>( this IList<T> source )
	{
		int index = Random.Range( 0, source.Count );

		return source[ index ];
	}

	public static T GetRandom<T>( this IEnumerable<T> source )
	{
		// ReSharper disable once PossibleMultipleEnumeration
		var index = Random.Range( 0, source.Count() );

		// ReSharper disable once PossibleMultipleEnumeration
		return source.ToArray()[ index ];
	}
}