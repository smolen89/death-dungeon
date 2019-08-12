
namespace RD.ECS.Systems.Movement
{
	public class MovementSystem
	{
		/*
		public MovementResult ExecutePassableMovement(IEntity entity, Direction direction )
		{
			// Sprawdzić czy da się wejść na pole
			return ExecuteMovement( entity, direction );
		}

		public MovementResult ExecuteMovement(IEntity entity, Direction movementDirection )
		{
			//! Check for modifications of the movement direction.
			if( entity.WillRespondTo( GameEventType.CheckMovementDirectionModification ) )
			{
				CheckMovementDirectionModificationGameEvent checkMovementDirectionModification
					= new CheckMovementDirectionModificationGameEvent(movementDirection);

				// entity wywołuje Event
				entity.PropagateEvent( checkMovementDirectionModification);

				// jeśli ktoś zmienił kierunek to trzeba to zgłosić
				if (checkMovementDirectionModification.direction != movementDirection )
				{
					// Signal to MessageSystem
					movementDirection = checkMovementDirectionModification.direction;
				}
			}

			//! Calculates the required positions.
			//Vec oldPosition = entity.Statistics.Position;
			//Vec newPosition = oldPosition + movementDirection;

			//if( !entity.Statistics.ZoneMap.IsValid( newPosition ) )
			//{
			//	return MovementResult.MovementAborted;
			//}

			//MapCell oldMapCell = entity.Statistics.ZoneMap.GetMapCellAt(oldPosition);
			//MapCell newMapCell = entity.Statistics.ZoneMap.GetMapCellAt(newPosition);


			//! Find iteractions that can be activated by movement and might replace the actual movement.

			//! Check correct/permissible movement.
			//if( !newMapCell.IsOpenFor( entity ) )
			//{
			//	return MovementResult.MovementFailed;
			//}

			//! Execute actions on the old map cell before we actuallu leave.

			//! Actually leave the old position.

			//! Execute actions before we enter the new position.

			//! Execute action on the new position.

			//! Actually enter the new position.

			//! Yup, we actually finished the movement.

			//! Onlu the moving entity is notified.

			//! Update the field of view

			//! Consime energy.

			//! Report success.
			return MovementResult.MovementSucceeded;
		}

		*/
	}
}