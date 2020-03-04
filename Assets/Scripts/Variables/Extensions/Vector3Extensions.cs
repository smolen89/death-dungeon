using UnityEngine;

public static class Vector3Extensions 
{
	public static Vector3 With( this Vector3 source, float? x = null, float? y = null, float? z = null )
	{
		var newX = x??source.x;
		var newY = y??source.y;
		var newZ = z??source.z;
		return new Vector3( newX, newY, newZ );
	}
}
