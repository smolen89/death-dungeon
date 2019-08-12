using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.ECS.Events_Tiny
{
	public struct EntityCreatedEvent : IEvent
	{
		public uint EntityID;
	}
}