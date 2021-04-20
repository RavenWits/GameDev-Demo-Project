using UnityEngine;

public class Outliner : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private BuildingController bc;

    private void Awake()
    {
        mat = this.GetComponent<SpriteRenderer>().material;

        if (bc == null)
        {
            bc = this.GetComponent<BuildingController>();
        }
    }

    void EnableOutline()
    {
        mat.SetFloat("_OutlineEnabled", 1f);
    }

    void DisableOutline()
    {
        mat.SetFloat("_OutlineEnabled", 0f);
    }

    private void OnMouseEnter()
    {

        EnableOutline();
        
    }

    private void OnMouseExit()
    {
        DisableOutline();
    }
}
