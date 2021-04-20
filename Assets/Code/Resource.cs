// Resource
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resource")]
public class Resource : ScriptableObject
{
	public string Name;
	public string Description;
	public Sprite Image;
	public ResourceType resource;

	[Range(1f, 60f)]
	[Tooltip("Select a value as minute betwwen 1 to 60 minutes.")]
	public int ProductionTime;
}

// ResourceType
public enum ResourceType { Empty, Food, Ore, Wood };
