using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class Building : ScriptableObject
{
      public string buildingName;
      public string buildingDescription;
      public Sprite buildingImage;
      public enum buildingType {Barracks, Farm};
      public bool hasProduction;
      public GameObject unit;


}
