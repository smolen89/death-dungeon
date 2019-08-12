using System;

namespace RD.ECS.Components
{
	[Serializable]
	public struct Health : IComponent
	{
		public string Name { get; set; }
		public int Value { get; set; }

		public string GetName() => nameof( Health );
	}

	public struct BodyPart : IComponent { }

	public struct Size : IComponent
	{
		public enum SizeType { Small, Medium, Huge }

		public SizeType Value;
	}

	public struct IsEnabled : IComponent
	{
	}

	public struct IsAlive : IComponent
	{
	}
}