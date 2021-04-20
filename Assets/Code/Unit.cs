using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
      public string unitName;
      public string unitDescription;
      public Sprite unitImage;
      public enum unitType {Soldier, Catapult};


}
