using UnityEngine;

[System.Serializable]
public struct RangeFloat
{
	public float minValue;
	public float maxValue;

	public RangeFloat( float min, float max )
	{
		minValue = min;
		maxValue = max;
	}

	public float GetRandom() => Random.Range( minValue, maxValue );
}