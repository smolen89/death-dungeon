using System.Collections.Generic;
using System.Numerics;
using RD.GameEngine.ECS.Systems;

namespace RD.GameEngine.ECS.Managers
{
	public class SystenBitManager
	{
		private readonly Dictionary<BaseEntitySystem, BigInteger> systemBits = new Dictionary<BaseEntitySystem, BigInteger>();
		private int position;

		public BigInteger GetBitFor( BaseEntitySystem entitySystem )
		{
			BigInteger bit;

			if ( systemBits.TryGetValue( entitySystem, out bit ) == false )
			{
				bit = new BigInteger(1) << this.position;
				position++;
				systemBits.Add( entitySystem, bit );
			}

			return bit;
		}
	}
}