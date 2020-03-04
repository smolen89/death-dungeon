using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class FloatNumber
{
	public NumberType type = NumberType.Value;
	public float value;
	public FloatNumber rangeMin;
	public FloatNumber rangeMax;
	public List<FloatNumber> sequence;
	public int sequenceIndex;
	public FloatNumber delta; //todo: some of these should be properties, with validation on changes.

	public static implicit operator float( FloatNumber n ) => n.GetValue();

	public float GetValue()
	{
		//also, need to enforce the requirement that every number eventually evaluates to a float.
		switch ( type )
		{
			case NumberType.Range:
			{
				while ( true )
				{
					float min = rangeMin.GetValue();
					float max = rangeMax.GetValue();
					float result = ( Random.value * ( max - min ) + min );

					if ( !float.IsInfinity( result ) && !float.IsNaN( result ) )
					{
						return result;
					}
				}
			}

			case NumberType.Sequence:
			{
				float result = sequence[ sequenceIndex ].GetValue();
				++sequenceIndex;

				if ( sequenceIndex == sequence.Count )
				{
					sequenceIndex = 0;
				}

				return result;
			}

			case NumberType.Delta:
			{
				float result = value;
				value += delta.GetValue();

				return result;
			}

			//case NumberType.Value:
			default:
				return value;
		}
	}

	public static FloatNumber CreateValue( float value )
	{
		FloatNumber n = new FloatNumber
		{
			value = value
		};

		return n;
	}

	public static FloatNumber CreateRange( FloatNumber min, FloatNumber max )
	{
		FloatNumber n = new FloatNumber
		{
			type = NumberType.Range,
			rangeMin = min,
			rangeMax = max
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
			type = NumberType.Sequence,
			sequence = new List<FloatNumber>()
		};

		return n;
	}

	public FloatNumber Add( float newValue )
	{
		if ( type != NumberType.Sequence )
		{
			throw new InvalidOperationException( "This method can only be used with a Sequence" );
		}

		sequence.Add( CreateValue( newValue ) );

		return this;
	}

	public FloatNumber Add( FloatNumber number )
	{
		if ( type != NumberType.Sequence )
		{
			throw new InvalidOperationException( "This method can only be used with a Sequence" );
		}

		sequence.Add( number );

		return this;
	}

	public static FloatNumber CreateDelta( float initialValue, FloatNumber delta )
	{
		FloatNumber n = new FloatNumber
		{
			type = NumberType.Delta,
			value = initialValue,
			delta = delta
		};

		return n;
	}

	public static FloatNumber CreateDelta( float initialValue, float delta ) 
		=> CreateDelta( initialValue, CreateValue( delta ) );
}