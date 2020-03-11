namespace RD.GameEngine.ECS.Components
{
	public enum ComponentFilterType
	{
		With,
		WithEither,
		
		Without,
		WithoutEither,
		
		WhenAdded,
		WhenAddedEither,
		
		WhenChanged,
		WhenChangedEither,
		
		WhenRemoved,
		WhenRemovedEither
	}
}