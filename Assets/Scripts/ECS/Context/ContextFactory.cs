using UnityEngine;
using System.Collections;

namespace RD.ECS.Contexts
{
	public class ContextFactory : IContextFactory
	{
		public IContext CreateNewContextInstance()
		{

			return new Context();
		}
	}
}