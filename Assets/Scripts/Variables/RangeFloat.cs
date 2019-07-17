using UnityEngine;

[System.Serializable]
public struct RangeFloat
{
	public float minValue;
	public float maxValue;

	public RangeFloat( float min, float max )
	{
		this.minValue = min;
		this.maxValue = max;
	}

	public float GetRandom() => Random.Range( minValue, maxValue );
}