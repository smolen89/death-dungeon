// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System;
using System.Collections.Generic;

public enum NumberType { Value, Range, Sequence, Delta };

//todo implement Random Seed
[Serializable]
public class Number
{
	public NumberType Type = NumberType.Value;
	public int Value = 0;
	public Number RangeMin = null;
	public Number RangeMax = null;
	public List<Number> Sequence = null;
	public int sequence_index = 0;
	public Number Delta = null; //todo: some of these should be properties, with validation on changes.

	public static implicit operator int( Number n ) => n.GetValue();

	public int GetValue()
	{
		//also, need to enforce the requirement that every number eventually evaluates to an int.
		switch( Type )
		{
			case NumberType.Range:

				return new Rand().RangeInclusive( RangeMin.GetValue(), RangeMax.GetValue() );

			case NumberType.Sequence:
				{
					int result = Sequence[sequence_index].GetValue();
					++sequence_index;
					if( sequence_index == Sequence.Count )
					{
						sequence_index = 0;
					}
					return result;
				}
			case NumberType.Delta:
				{
					int result = Value;
					Value += Delta.GetValue();
					return result;
				}
			case NumberType.Value:
			default:
				return Value;
		}
	}

	public static Number CreateValue( int value )
	{
		Number n = new Number
		{
			Value = value
		};
		return n;
	}

	public static Number CreateRange( Number min, Number max )
	{
		Number n = new Number
		{
			Type = NumberType.Range,
			RangeMin = min,
			RangeMax = max
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
			Type = NumberType.Sequence,
			Sequence = new List<Number>()
		};
		return n;
	}

	public Number Add( int value )
	{
		if( Type != NumberType.Sequence )
		{
			throw new InvalidOperationException( "This method can only be used with a Sequence" );
		}
		Sequence.Add( CreateValue( value ) );
		return this;
	}

	public Number Add( Number number )
	{
		if( Type != NumberType.Sequence )
		{
			throw new InvalidOperationException( "This method can only be used with a Sequence" );
		}
		Sequence.Add( number );
		return this;
	}

	public static Number CreateDelta( int initial_value, Number delta )
	{
		Number n = new Number
		{
			Type = NumberType.Delta,
			Value = initial_value,
			Delta = delta
		};
		return n;
	}

	public static Number CreateDelta( int initial_value, int delta ) => CreateDelta( initial_value, CreateValue( delta ) );
}

public class FloatNumber
{
	public NumberType Type = NumberType.Value;
	public float Value = 0;
	public FloatNumber RangeMin = null;
	public FloatNumber RangeMax = null;
	public List<FloatNumber> Sequence = null;
	public int sequence_index = 0;
	public FloatNumber Delta = null; //todo: some of these should be properties, with validation on changes.

	public static implicit operator float( FloatNumber n ) => n.GetValue();

	public float GetValue()
	{ //also, need to enforce the requirement that every number eventually evaluates to a float.
		switch( Type )
		{
			case NumberType.Range:
				{
					while( true )
					{
						float min = RangeMin.GetValue();
						float max = RangeMax.GetValue();
						float result = (float)(UnityEngine.Random.value * (double)(max-min) + (double)min);
						if( !float.IsInfinity( result ) && !float.IsNaN( result ) )
						{
							return result;
						}
					}
				}
			case NumberType.Sequence:
				{
					float result = Sequence[sequence_index].GetValue();
					++sequence_index;
					if( sequence_index == Sequence.Count )
					{
						sequence_index = 0;
					}
					return result;
				}
			case NumberType.Delta:
				{
					float result = Value;
					Value += Delta.GetValue();
					return result;
				}
			case NumberType.Value:
			default:
				return Value;
		}
	}

	public static FloatNumber CreateValue( float value )
	{
		FloatNumber n = new FloatNumber
		{
			Value = value
		};
		return n;
	}

	public static FloatNumber CreateRange( FloatNumber min, FloatNumber max )
	{
		FloatNumber n = new FloatNumber
		{
			Type = NumberType.Range,
			RangeMin = min,
			RangeMax = max
		};
		return n;
	}

	public static FloatNumber CreateRange( float min, float max ) => CreateRange( CreateValue( min ), CreateValue( max ) );

	public static FloatNumber CreateRange( FloatNumber min, float max ) => CreateRange( min, CreateValue( max ) );

	public static FloatNumber CreateRange( float min, FloatNumber max ) => CreateRange( CreateValue( min ), max );

	public static FloatNumber CreateSequence()
	{
		FloatNumber n = new FloatNumber
		{
			Type = NumberType.Sequence,
			Sequence = new List<FloatNumber>()
		};
		return n;
	}

	public FloatNumber Add( float value )
	{
		if( Type != NumberType.Sequence )
		{
			throw new InvalidOperationException( "This method can only be used with a Sequence" );
		}
		Sequence.Add( CreateValue( value ) );
		return this;
	}

	public FloatNumber Add( FloatNumber number )
	{
		if( Type != NumberType.Sequence )
		{
			throw new InvalidOperationException( "This method can only be used with a Sequence" );
		}
		Sequence.Add( number );
		return this;
	}

	public static FloatNumber CreateDelta( float initial_value, FloatNumber delta )
	{
		FloatNumber n = new FloatNumber
		{
			Type = NumberType.Delta,
			Value = initial_value,
			Delta = delta
		};
		return n;
	}

	public static FloatNumber CreateDelta( float initial_value, float delta ) => CreateDelta( initial_value, CreateValue( delta ) );
}