
![GitHub top language](https://img.shields.io/github/languages/top/smolen89/death-dungeon.svg?style=plastic)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/smolen89/death-dungeon.svg?color=darkcyan&style=plastic)
![GitHub pull requests](https://img.shields.io/github/issues-pr/smolen89/death-dungeon.svg?style=plastic)
![GitHub issues](https://img.shields.io/github/issues/smolen89/death-dungeon.svg?style=plastic)
![GitHub commit activity](https://img.shields.io/github/commit-activity/w/smolen89/death-dungeon.svg?style=plastic)
![GitHub](https://img.shields.io/github/license/smolen89/death-dungeon.svg?style=plastic)
# Death Dungeon

 (_Robocza nazwa - trzeba zmienić_)

> Gra w stylu Roguelike na silniku Unity.

# TODO List

- Input & movements
- Commands for movement, save etc
- Events for well, events
- Player stats
- Map structure, format, display
- Level generatos
- Collisions
- Objects: treasure chests, keys
- Random encounters
- Monsters
- Data files: hard-coded lookup tables
- MAgic
- Tuning
- Shop keepers
- Citizens



UISignal - From RDE (ECS) to UI

SystemCall/Query - From UI to RDE(ECS)

SystemCall - From Component to System

Rest is GameEvent


public MovementResult ExecuteMovement(IEntity entity, Direction direction)
{
    if (entity.WillRespondTo(GameEventType.CheckMovementDirection))
    {
        CheckDirectionGameEvent cdge = new CheckDirectionGameEvent(direction)
        entity.PropagateEvent(cdge);
        if (cdge.MovementDirection != direction)
        {
            RDE.Engine.MessageSystem.Signal(new AppliedEffectIsObservableUISignalDetal(cdge.CauseOfDirectionChange,entity),
            cdge.LongMessage,
            cdge.ShortMessage,
            entity);
            direction = cdge.MovementDirection;
        }
    }
}
