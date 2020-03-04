using UnityEngine;

[System.Serializable]
public struct RangeInt
{
	public int minValue;
	public int maxValue;

	public RangeInt( int min, int max )
	{
		this.minValue = min;
		this.maxValue = max;
	}

	public int GetRandom() => Random.Range( minValue, maxValue );
}