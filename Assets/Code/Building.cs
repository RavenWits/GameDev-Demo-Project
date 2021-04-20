// Building
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class Building : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Image;
    public BuildingType type;
    public int Durability;
    public int Width;
    public int Height;
    public Unit[] Units;
    public Resource resource;
}

// BuildingType
public enum BuildingType{ Empty, UnitProduction, ResourceProduction }