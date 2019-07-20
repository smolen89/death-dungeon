// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
	public static T GetRandom<T>( this IList<T> source )
	{
		int index = Random.Range( 0, source.Count );
		return source[index];
	}

	// Prawdopodobnie IList załatwił sprawę, choć Linq się może przydać
	public static T GetRandom<T>( this IEnumerable<T> source )
	{
		int index = Random.Range(0,source.Count());
		return source.ToArray()[ index ];
	}
}