// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;
using System.Collections.Generic;
using System;
using URand = UnityEngine.Random;
using SRand = System.Random;

//bug ZAWOLNE!!!!!
//! pomysły na Rand:
// - zrobić osobne randy na typy działań. np do tworzenia itemów jeden do generowania map inny, do generowania enemy inny itp.
// - [PREFER] Zamienić UnityRandom na SystemRandom, a unityRandom używać do takich rzeczy jak animacje czy particles gdzie nie ma znaczenia jaki seed będzie.
//	 to rozwiązanie pozwoli pozbyć się Stanów. jednak żeby zablokować reload randoma
//	 załóżmy że mamy chest. seed dla tego chesta będzie coś w stylu chest.GetHashCode()
/// <summary>
/// The Random Number God.
/// </summary>
[Serializable]
public class Rand : ISerializationCallbackReceiver
{
	public Rand( int seed = -1 )
	{
		if( seed < 0 )
		{
			this.seed = Environment.TickCount;
		}
		else
		{
			this.seed = seed;
		}
		random = new SRand( seed );
		count = 0;
	}

	[SerializeField]    private int seed;
	[SerializeField]    private int count;
	[SerializeField]    private SRand random;

	//private static int GetRewardID(int seed, int HashFromCoord, int numRewards )
	//{
	//	int tempSeed = BitConverter.ToInt32(BitConverter.GetBytes(coord.GetHashCode),0)^seed;
	//	return new System.Random( tempSeed ).Next( numRewards );
	//}

	//int seed = new System.Random().Next();
	//int numDiffrentRewards = 5;
	//int xCoord = new Vec().GetHashCode();
	//int rewardID = GetRewardID(seed,xCoord,numDiffrentRewards);

	public float value
	{
		get
		{
			count++;
			return (float)random.NextDouble();
		}
	}

	/// <summary>
	/// Gets a random int between 0 and max (half-inclusive).
	/// </summary>
	public int Range( int max )
	{
		count++;
		return random.Next( max );
	}

	public int Range( int min, int max )
	{
		return Range( max - min ) + min;
	}

	public int RangeInclusive( int max )
	{
		count++;
		return random.Next( max + 1 );
	}

	public int RangeInclusive( int min, int max )
	{
		return RangeInclusive( max - min ) + min;
	}

	public float Range( float max )
	{
		if( max < 0.0f ) Debugger.Log( "Rand", $"Range() - The max must be zero or greater." );
		count++;
		return (float)random.NextDouble() * max;
	}

	public float Range( float min, float max )
	{
		if( max < min ) Debugger.LogError( "Rand", "Range() - The max must be min or greater." );

		return Range( max - min ) + min;
	}

	public Vec Range( Vec size )
	{
		return new Vec( Range( size.x ), Range( size.y ) );
	}

	public Vec RangeInclusive( Vec size )
	{
		return new Vec( Range( size.x ), Range( size.y ) );
	}

	public Vec Range( Rect rect )
	{
		return new Vec( Range( rect.Left, rect.Right ), Range( rect.Top, rect.Bottom ) );
	}

	public bool OneIn( int chance )
	{
		return Range( chance ) == 0;
	}

	public bool FractionalChance( int x, int outOfY )
	{
		if( x >= outOfY )
		{
			return true;
		}

		return Range( 1, outOfY ) <= x;
	}

	public bool Chance( int probabilityFactor, int probabilitySpace )
	{
		return Range( probabilitySpace ) < probabilityFactor ? true : false;
	}

	public List<T> Shuffle<T>( List<T> list )
	{
		List<T> shuffledList = new List<T>(0);
		int nListCount = list.Count;
		int nElementIndex;

		for( int i = 0; i < nListCount; i++ )
		{
			nElementIndex = Range( list.Count );
			shuffledList.Add( list[nElementIndex] );
			list.RemoveAt( nElementIndex );
		}

		return shuffledList;
	}

	public T[] Shuffle<T>( T[] array )
	{
		T[] shuffledArray = new T[array.Length];
		List<int> elementIndices = new List<int>(0);

		for( int i = 0; i < array.Length; i++ )
		{
			elementIndices.Add( i );
		}

		int nArrayIndex;

		for( int i = 0; i < array.Length; i++ )
		{
			nArrayIndex = elementIndices[Range( elementIndices.Count )];
			shuffledArray[i] = array[nArrayIndex];
			elementIndices.Remove( nArrayIndex );
		}

		return shuffledArray;
	}

	public T Choice<T>( T[] array )
	{
		return array[Range( array.Length )];
	}

	public T Choice<T>( List<T> list )
	{
		return list[Range( list.Count )];
	}

	public T WeightedChoice<T>( T[] array, int[] weights )
	{
		int nTotalWeight = 0;
		for( int i = 0; i < array.Length; i++ )
		{
			nTotalWeight += weights[i];
		}
		int nChoiceIndex = Range( nTotalWeight);
		for( int i = 0; i < array.Length; i++ )
		{
			if( nChoiceIndex < weights[i] )
			{
				nChoiceIndex = i;
				break;
			}
			nChoiceIndex -= weights[i];
		}
		return array[nChoiceIndex];
	}

	public T WeightedChoice<T>( List<T> list, int[] weights )
	{
		int nTotalWeight = 0;
		for( int i = 0; i < list.Count; i++ )
		{
			nTotalWeight += weights[i];
		}
		int nChoiceIndex = Range(nTotalWeight);
		for( int i = 0; i < list.Count; i++ )
		{
			if( nChoiceIndex < weights[i] )
			{
				nChoiceIndex = i;
				break;
			}
			nChoiceIndex -= weights[i];
		}
		return list[nChoiceIndex];
	}

	public int WeightedChoiceIndex( List<int> weights )
	{
		int total = 0;

		foreach( int n in weights )
		{
			total += n;
		}

		int idx = 0;

		for( int roll = Range( 1, total + 1 ); roll > 0; )
		{
			roll -= weights[idx++];
		}

		return idx - 1;
	}

	public int WeightedChoiceIndex( int[] weights )
	{
		int total = 0;

		foreach( int n in weights )
		{
			total += n;
		}

		int idx = 0;

		for( int roll = Range( 1, total + 1 ); roll > 0; )
		{
			roll -= weights[idx++];
		}

		return idx - 1;
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		random = new SRand( seed );
		for( int i = 0; i < count; i++ )
		{
			random.Next();
		}
	}
}