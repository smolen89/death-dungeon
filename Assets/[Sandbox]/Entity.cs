using UnityEngine;
using System.Collections;

namespace RD.ECS
{

	public interface IEntity
	{

	}
	public interface IComponent
	{

	}


	/// <summary>
	/// Identifier + Data
	/// </summary>
	public class Entity : IEntity
	{
		string ID;
		string BaseTemplate;    // to Entity ID from componentTemplates
		string Type;

		Component[] components;
		Entity[] Slots;
		Entity[] Container;
	}




	/// <summary>
	/// Init Data + Simple Behaviour
	/// </summary>
	public class Component : IComponent
	{
		string componentType = "Solid";
		Entity[] Slots;

	}






	/// <summary>
	/// Complex & reusable Behaviour
	/// </summary>
	public class System
	{
		
	}







	public static class EntityTemplates
	{

	}
}