// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.

using System;
using System.Collections.Generic;

public enum NumberType
{
	Value,
	Range,
	Sequence,
	Delta
}

//todo implement Random Seed
[Serializable]
public class Number
{
	public NumberType type = NumberType.Value;
	public int value;
	public Number rangeMin;
	public Number rangeMax;
	public List<Number> sequence;
	public int sequenceIndex;
	public Number delta; //todo: some of these should be properties, with validation on changes.

	public static implicit operator int( Number n ) => n.GetValue();

	public int GetValue()
	{
		//also, need to enforce the requirement that every number eventually evaluates to an int.
		switch ( type )
		{
			case NumberType.Range:

				return new Rand().RangeInclusive( rangeMin.GetValue(), rangeMax.GetValue() );

			case NumberType.Sequence:
			{
				int result = sequence[ sequenceIndex ].GetValue();
				++sequenceIndex;

				if ( sequenceIndex == sequence.Count )
				{
					sequenceIndex = 0;
				}

				return result;
			}

			case NumberType.Delta:
			{
				int result = value;
				value += delta.GetValue();

				return result;
			}

			//case NumberType.Value:
			default:
				return value;
		}
	}

	public static Number CreateValue( int value )
	{
		Number n = new Number
		{
			value = value
		};

		return n;
	}

	public static Number CreateRange( Number min, Number max )
	{
		Number n = new Number
		{
			type = NumberType.Range,
			rangeMin = min,
			rangeMax = max
		};

		return n;
	}

	public static Number CreateRange( int min, int max ) => CreateRange( CreateValue( min ), CreateValue( max ) );

	public static Number CreateRange( Number min, int max ) => CreateRange( min, CreateValue( max ) );

	public static Number CreateRange( int min, Number max ) => CreateRange( CreateValue( min ), max );

	public static Number CreateSequence()
	{
		Number n = new Number
		{
			type = NumberType.Sequence,
			sequence = new List<Number>()
		};

		return n;
	}

	public Number Add( int newValue )
	{
		if ( type != NumberType.Sequence )
		{
			throw new InvalidOperationException( "This method can only be used with a Sequence" );
		}

		sequence.Add( CreateValue( newValue ) );

		return this;
	}

	public Number Add( Number number )
	{
		if ( type != NumberType.Sequence )
		{
			throw new InvalidOperationException( "This method can only be used with a Sequence" );
		}

		sequence.Add( number );

		return this;
	}

	public static Number CreateDelta( int initialValue, Number delta )
	{
		Number n = new Number
		{
			type = NumberType.Delta,
			value = initialValue,
			delta = delta
		};

		return n;
	}

	public static Number CreateDelta( int initialValue, int delta ) => CreateDelta( initialValue, CreateValue( delta ) );
}