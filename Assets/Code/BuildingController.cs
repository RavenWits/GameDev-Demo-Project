using UnityEngine;
using System.Collections.Generic;

public class BuildingController : MonoBehaviour
{
    private Color red = new Color(1, 0, 0, 0.75f), green = new Color(0, 1, 0, 0.75f);
    private SpriteRenderer sr;
    private BoxCollider2D coll;
    public Building building;
    [SerializeField] bool isPlaceable = false, onGrass = false, onWater = false, onBuilding = false, onUnit = false;
    public bool isPlaced = false;
    public delegate void ClickAction(Building data, GameObject buildingObject);
    public static event ClickAction onClicked;
    public List<GameObject> spawnPoints;
    public Vector3Int spawnPos;
    public GameObject soldier;

    private void OnEnable()
    {
        SpawnBuilding.onClicked += BuildMode;

        sr = this.GetComponent<SpriteRenderer>();
        coll = this.GetComponent<BoxCollider2D>();
    }

    void BuildMode()
    {
        sr.color = green;
        GameController.instance.buildMode = true;
        this.isPlaceable = false;
        SpawnBuilding.onClicked -= BuildMode;
    }

    private void Update()
    {
        if (GameController.instance.buildMode && !isPlaced)
        {
            transform.position = GameController.instance.truePos;

            sr.color = (isPlaceable) ? green : red;

            if (Input.GetMouseButtonDown(1))
            {
                Vector3Int gridPos = GameController.instance.map.WorldToCell(GameController.instance.truePos);
                if (GameController.instance.map.HasTile(gridPos) && isPlaceable)
                {
                    GameController.instance.buildMode = false;
                    this.isPlaceable = false;
                    this.isPlaced = true;
                    sr.color = Color.white;
                    sr.sortingOrder = 1;
                    ActivateSpawnPoints();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag(Tags.water))
        {
            onWater = true;
        }

        if (other.gameObject.layer.Equals(LayerMask.NameToLayer(Tags.building)))
        {
            onBuilding = true;
        }

        if (other.gameObject.CompareTag(Tags.unit))
        {
            onUnit = true;
        }

        if (onWater || onBuilding || onUnit)
        {
            isPlaceable = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.grass) && !onWater && !onBuilding && !onUnit && !isPlaced)
        {
            if (!onGrass)
            {
                onGrass = true;
            }

            if (!isPlaceable)
            {
                isPlaceable = true;
            }
        }

        if (other.gameObject.layer.Equals(LayerMask.NameToLayer(Tags.building)))
        {
            onBuilding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.layer.Equals(LayerMask.NameToLayer(Tags.building)))
        {
            onBuilding = false;
        }

        if (other.gameObject.CompareTag(Tags.water))
        {
            onWater = false;
        }

        if (other.gameObject.CompareTag(Tags.grass))
        {
            onGrass = false;
        }

        if (other.gameObject.CompareTag(Tags.unit))
        {
            onUnit = false;
        }

        if (!onGrass)
        {
            isPlaceable = false;
        }

    }

    private void OnMouseDown()
    {
        if (onClicked != null && isPlaced)
        {
            onClicked(building, gameObject);
        }
    }

    private void ActivateSpawnPoints()
    {
        if (building.hasProduction)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf.Equals(false))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }




}
