using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions 
{
	public static Vector3 With( this Vector3 source, float? x = null, float? y = null, float? z = null )
	{
		float newX = x??source.x;
		float newY = y??source.y;
		float newZ = z??source.z;
		return new Vector3( newX, newY, newZ );
	}
}
