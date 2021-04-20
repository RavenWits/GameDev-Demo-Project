// BuildingController
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
	public delegate void ClickAction(Building data, GameObject buildingObject);
	public static event ClickAction onClicked;
	private Color red = new Color(1f, 0f, 0f, 0.75f), green = new Color(0f, 1f, 0f, 0.75f);
	private SpriteRenderer sr;
	private BoxCollider2D coll;
	private Rigidbody2D rb;
	public Building building;
	[SerializeField] bool isPlaceable = false, ongrass = false, onWater = false, onBuilding = false, onUnit = false ;
	public bool isPlaced = false;
	public List<GameObject> spawnPoints;
	public Vector3Int spawnPos;
	public GameObject soldier;
	public int currentDurability;
	public int CurrentDurability
	{
		get
		{
			return currentDurability;
		}
		set
		{
			currentDurability = building.Durability;
		}
	}



	//Registering to events on enable.
	private void OnEnable()
	{
		SpawnBuilding.onClicked += BuildMode;
		sr = GetComponent<SpriteRenderer>();
		coll = GetComponent<BoxCollider2D>();
		CurrentDurability = building.Durability;
	}

	//Fires the event to the subscribers on click.
	private void OnMouseDown()
	{
		if (BuildingController.onClicked != null && isPlaced)
		{
			BuildingController.onClicked(building, base.gameObject);
		}
	}

	//Triggers the state of build mode.
	private void BuildMode(GameObject gameObject)
	{
		sr.color = green;
		GameController.instance.buildMode = true;
		isPlaceable = false;
		SpawnBuilding.onClicked -= BuildMode;
	}

	private void Update()
	{
		if (GameController.instance.buildMode || !isPlaced)
		{
			transform.position = GameController.instance.truePos;
			sr.color = (isPlaceable ? green : red);

			if (Input.GetMouseButtonDown(1))
			{
				PlaceBuilding();
			}
		}

        if (onWater || onBuilding || onUnit)
		{
			isPlaceable = false;
		}
		if (!onWater && !onBuilding && !onUnit && onGrass && !isPlaceable)
		{
			isPlaceable = true;
		}
	}

	//Checks the grid.. if it is an appropriate tile proceeds with placing a building.
    private void PlaceBuilding()
    {
        Vector3Int gridPos = GameController.instance.map.WorldToCell(GameController.instance.truePos);
        if (GameController.instance.map.HasTile(gridPos) && isPlaceable)
        {
            GameController.instance.buildMode = false;
            isPlaceable = false;
            isPlaced = true;
            sr.color = Color.white;
            sr.sortingOrder = 1;
            ActivateSpawnPoints();
            UIController.instance.EnableBuildingButtons();
        }
    }

	//Checks if the building has a production feature, if so triggers the spawn points.
	private void ActivateSpawnPoints()
	{
		if (!building.type.Equals(BuildingType.UnitProduction))
		{
			return;
		}
		foreach (Transform spawnpoint in base.transform)
		{
			if (spawnpoint.gameObject.activeSelf.Equals(false))
			{
				spawnpoint.gameObject.SetActive(true);
			}
		}
	}

    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Building")))
		{
			onBuilding = true;
		}
		if (other.gameObject.CompareTag("Water"))
		{
			onWater = true;
		}
		if (other.gameObject.CompareTag("Unit"))
		{
			onUnit = true;
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Grass") && !onWater && !onBuilding && !onUnit && !isPlaced && !onGrass)
		{
			onGrass = true;
		}
		if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Building")))
		{
			onBuilding = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Building")))
		{
			onBuilding = false;
		}
		if (other.gameObject.CompareTag("Water"))
		{
			onWater = false;
		}
		if (other.gameObject.CompareTag("Grass"))
		{
			onGrass = false;
		}
		if (other.gameObject.CompareTag("Unit"))
		{
			onUnit = false;
		}
		if (!onGrass)
		{
			isPlaceable = false;
		}
	}



}
