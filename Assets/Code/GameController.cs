// GameController
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Tilemap map;
    public Vector2 mousePos, truePos;
    public float gridSize = 12f;
    public bool buildMode = false;
    public static bool isPaused;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (map == null)
        {
            map = GameObject.FindWithTag("Grass").GetComponent<Tilemap>();
        }
    }

    private void Update()
    {
		//Calculates real world grid position based on mouse position only on build mode.
        if ((buildMode || UIController.instance.selectedObject != null) && !isPaused)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            truePos.x = Mathf.Floor(mousePos.x / gridSize) * gridSize;
            truePos.y = Mathf.Floor(mousePos.y / gridSize) * gridSize;
        }

        if (Input.GetMouseButtonDown(0))
        {
            CheckObject();
        }
    }

	//Checks the clicked Game Object if it is a building or unit makes it currently selected object.
    private void CheckObject()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(mousePos, Vector2.zero, float.PositiveInfinity);
        if (rayHit.collider != null && !rayHit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Building")) && !rayHit.collider.gameObject.CompareTag("Unit"))
        {
            UIController.instance.SelectObject(null);
        }
    }
}
