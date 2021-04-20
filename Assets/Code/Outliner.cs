// Outliner
using UnityEngine;

/// <summary>
/// This is a basic script to toggle an outline to make player understand which object is currently selected.
/// </summary>
public class Outliner : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private BuildingController bc;

    private void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
        if (bc == null)
        {
            bc = GetComponent<BuildingController>();
        }
    }

    public void EnableOutline(Color color)
    {
        mat.SetFloat("_OutlineEnabled", 1f);
        mat.SetColor("_SolidOutline", color);
    }

    public void DisableOutline()
    {
        mat.SetFloat("_OutlineEnabled", 0f);
    }

    public bool isOutlined()
    {
        if (mat.GetFloat("_OutlineEnabled").Equals(0f))
        {
            return false;
        }
        return true;
    }

    private void OnMouseEnter()
    {
        if (UIController.instance.selectedObject != base.gameObject)
        {
            EnableOutline(Color.white);
        }
    }

    private void OnMouseExit()
    {
        if (UIController.instance.selectedObject != base.gameObject)
        {
            DisableOutline();
        }
    }
}
