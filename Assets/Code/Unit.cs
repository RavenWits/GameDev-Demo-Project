// Unit
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
	public enum Type
	{
		Soldier,
		Catapult
	}

	public string Name;

	public string Description;

	public Sprite Image;

	public int resource;

	public int health;
}
