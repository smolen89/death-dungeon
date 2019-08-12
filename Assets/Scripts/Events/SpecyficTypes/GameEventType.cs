// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi


namespace RD.GameEngine.Events
{
	public enum GameEventType
	{
		CheckMovementDirectionModification,
		FindMovementIteractions,
		BeforeLeaveOldPosition,
		LeaveOldPosition,
		BeforeEnterNewPosition,
		EnterNewPosition,
		FinishedMovementToNewPosition,



		HasNotFoundEmptySlotForPickingUpItems,


		FindLongHurtMessage,
	}
}