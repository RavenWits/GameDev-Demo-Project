using UnityEngine;
using UnityEngine.Tilemaps;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Tilemap map;
    public Vector2 mousePos, truePos, truePosMobile;
    private float gridSize = 12f;
    public bool buildMode = false;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        if (map == null)
        {
            map = GameObject.FindWithTag(Tags.grass).GetComponent<Tilemap>();
        }
    }

    void Update()
    {
        if(buildMode || UIController.instance.selectedObject != null)
        //Get current Mouse Coordinates
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        truePos.x = Mathf.Floor(mousePos.x / gridSize) * gridSize;
        truePos.y = Mathf.Floor(mousePos.y / gridSize) * gridSize;

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            truePosMobile.x = Mathf.Floor(touch.position.x / gridSize) * gridSize;
            truePosMobile.y = Mathf.Floor(touch.position.y / gridSize) * gridSize;
        }
    }

}