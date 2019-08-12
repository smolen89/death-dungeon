using UnityEngine;
using System.Collections;
using System;

namespace RD.ECS.Message
{
	[AttributeUsage( AttributeTargets.Method )]
	public sealed class RegisterListenerAttribute : Attribute
	{
	}
}