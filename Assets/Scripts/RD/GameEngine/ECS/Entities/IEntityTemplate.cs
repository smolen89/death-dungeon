using RD.GameEngine.ECS.Entities;

namespace RD.GameEngine.ECS.Entities
{
	public interface IEntityTemplate
	{
		void TryGetValue( string entityTemplateTag, out IEntityTemplate entityTemplate );
		IEntity BuildEntity( IEntity entity, World world, object[] templateArgs );
	}
}