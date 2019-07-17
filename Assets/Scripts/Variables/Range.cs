using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Range
{
	public int min;
	public int max;

	public Range(int min, int max )
	{
		this.min = min;
		this.max = max;
	}
}
