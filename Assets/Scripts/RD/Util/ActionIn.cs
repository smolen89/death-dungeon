namespace RD.Util
{
	/// <summary>
	/// Encapsulates a method that has a single in parameter and does not return a value used for <see cref="World.Subscribe{T}(ActionIn{T})"/> method.
	/// </summary>
	/// <typeparam name="T">The type of message to subscribe to.</typeparam>
	public delegate void ActionIn<T>(in T message);
}