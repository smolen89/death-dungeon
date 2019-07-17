using System;

public class RangeFloatMinMaxAttribute : Attribute
{
	public RangeFloatMinMaxAttribute( float min, float max )
	{
		this.Min = min;
		this.Max = max;
	}

	public float Min { get; private set; }
	public float Max { get; private set; }
}
