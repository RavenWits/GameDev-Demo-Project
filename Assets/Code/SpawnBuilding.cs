using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBuilding : MonoBehaviour
{
    [SerializeField] GameObject building;
   
    public delegate void ClickAction();
    public static event ClickAction onClicked;
    public string tagName;

    void Start()
    {
        tagName = this.tag;
        building = ObjectPooler.SharedInstance.GetPooledObject(tagName);
    }

    private void OnMouseDown()
    {
        if (building != null)
        {
            building.transform.position = transform.position;
            building.SetActive(true);
            
            building = null;          

            if (onClicked != null)
            {
                onClicked();
            }
        }
    }

    private void OnMouseUp() {
        if(building == null)
        {
            building = ObjectPooler.SharedInstance.GetPooledObject(tagName);
        }
    }



}
