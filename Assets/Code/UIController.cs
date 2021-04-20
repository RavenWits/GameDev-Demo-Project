// UIController
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public static UIController instance;
	[SerializeField] private Building building;
	[SerializeField] private Unit unit;
	[SerializeField] private TMP_Text selectedName, selectedDesc, selectedDurability, productName, productType, productDesc, productRate, unitHealth, unitResource, unitName, unitType;
	[SerializeField] private Image selectedImage, productImage, durabilityImage;
	[SerializeField] private Sprite heartIcon, towerIcon;
	[SerializeField] private GameObject UnitProduction, ResourceProduction, content, alert;
	[SerializeField] private Transform BuildingList;
	[SerializeField] private Texture2D cursorTexture;
	private BuildingController bc;
	private UnitController uc;
	private Transform[] allButtons;
	public GameObject selectedObject;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	//Registering to the events.
	private void Start()
	{
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
		allButtons = BuildingList.GetComponentsInChildren<Transform>();

		BuildingController.onClicked += SetFieldsBuilding;
		UnitController.onClicked += SetFieldsUnit;
		SpawnBuilding.onClicked += SelectObject;
	}

	//Fetches UI data of the selected building according to the building type.
	private void SetFieldsBuilding(Building data, GameObject buildingObject)
	{
		if (GameController.isPaused.Equals(false))
		{
			SelectObject(buildingObject);
			bc = selectedObject.GetComponent<BuildingController>();
			building = data;
			content.SetActive(true);
			selectedName.text = building.Name;
			selectedImage.sprite = building.Image;
			durabilityImage.sprite = towerIcon;
			selectedDesc.text = building.Description;
			selectedDurability.text = bc.currentDurability + "/" + building.Durability.ToString();
			if (building.type.Equals(BuildingType.UnitProduction))
			{
				unitName.text = building.Units[0].Name;
				unitType.text = Enum.GetName(typeof(Unit.Type), 0).ToString();
				unitHealth.text = building.Units[0].health.ToString();
				unitResource.text = building.Units[0].resource.ToString();
				ResourceProduction.SetActive(false);
				UnitProduction.SetActive(true);
			}
			if (building.type.Equals(BuildingType.ResourceProduction))
			{
				productName.text = building.resource.Name;
				productType.text = Enum.GetName(typeof(ResourceType), 1).ToString();
				productDesc.text = building.resource.Description;
				productImage.sprite = building.resource.Image;
				productRate.text = building.resource.ProductionTime + ":00";
				UnitProduction.SetActive(false);
				ResourceProduction.SetActive(true);
			}
		}
	}

	//Fetches UI data of the selected unit according to the unit type.
	private void SetFieldsUnit(Unit data, GameObject unitObject)
	{
		if (GameController.isPaused.Equals(false))
		{
			if (selectedObject != null && selectedObject.CompareTag("Unit"))
			{
				uc.target.SetActive(false);
			}
			SelectObject(unitObject);
			uc = selectedObject.GetComponent<UnitController>();
			uc.target.SetActive(true);
			unit = uc.unit;
			content.SetActive(true);
			selectedName.text = unit.Name;
			selectedImage.sprite = unit.Image;
			selectedDesc.text = unit.Description;
			durabilityImage.sprite = heartIcon;
			selectedDurability.text = uc.currentHealth + "/" + data.health.ToString();
			ResourceProduction.SetActive(false);
			UnitProduction.SetActive(false);
		}
	}

	//Using this method While on build mode to disable other building buttons to prevent stacking.
	public void DisableBuildingButtons()
	{
		Transform[] array = allButtons;
		foreach (Transform button in array)
		{
			button.GetComponentInChildren<Button>().interactable = false;
		}
	}

	//Using this method after placing a building on gameboard to enable other building buttons.
	public void EnableBuildingButtons()
	{
		Transform[] array = allButtons;
		foreach (Transform button in array)
		{
			button.GetComponentInChildren<Button>().interactable = true;
		}
	}

	//Using this method to close off detail panel if clicked outside of an object.
	public void CloseRightPanel()
	{
		if (content.activeInHierarchy.Equals(true) && GameController.isPaused.Equals(false))
		{
			content.SetActive(false);
		}
	}

	//
	public void SelectObject(GameObject gameObject)
	{
		//Deselects currently selected object and closes detail UI panel.
		if (gameObject == null)
		{
			if (selectedObject != null)
			{
				if (selectedObject.CompareTag("Unit"))
				{
					selectedObject.GetComponent<UnitController>().target.SetActive(false);
				}
				selectedObject.GetComponent<Outliner>().DisableOutline();
			}
			CloseRightPanel();
		}


		if (selectedObject != null)
		{
			selectedObject.GetComponent<Outliner>().DisableOutline();
		}

		selectedObject = gameObject;

		//Outlines selected object with different color to make visual difference between hover and click.
		if (selectedObject != null)
		{
			selectedObject.GetComponent<Outliner>().EnableOutline(Color.yellow);
		}
	}

	//Gets proper unit type from pool using tags. Then spawns the unit to a random available spawn point.
	public void SpawnSoldier(string tag)
	{
		bc.soldier = ObjectPooler.SharedInstance.GetPooledObject(tag);
		int rnd = UnityEngine.Random.Range(0, bc.spawnPoints.Count);
		bc.spawnPos = Vector3Int.FloorToInt(bc.spawnPoints[rnd].transform.position);
		bc.soldier.transform.position = bc.spawnPos;
		bc.soldier.SetActive(true);
		bc.soldier = null;
	}

	//Checks currently available spawn points of the building. If there is a valid spawn point proceeds with spawning. If there is no available spawn point fires an indicator alert to player.
	public void CheckSpawnPoints(string tag)
	{
		foreach (Transform child in selectedObject.transform)
		{
			if (!child.GetComponent<spawnPoint>().canSpawn)
			{
				if (bc.spawnPoints.Contains(child.gameObject))
				{
					bc.spawnPoints.Remove(child.gameObject);
				}
			}
			else if (!bc.spawnPoints.Contains(child.gameObject))
			{
				bc.spawnPoints.Add(child.gameObject);
			}
		}
		if (bc.spawnPoints.Count != 0 && !GameController.isPaused)
		{
			SpawnSoldier(tag);
			return;
		}
		Debug.LogWarning("No available space to spawn a unit!");
		StartCoroutine(showAlert());
	}

	//Using coroutines here because project documentation requires me to.
	public void HideAlert()
	{
		StartCoroutine(hideAlert());
	}

	private IEnumerator showAlert()
	{
		alert.SetActive(true);
		GameController.isPaused = true;
		while (alert.transform.localScale.magnitude < Vector3.one.magnitude - 0.1f)
		{
			alert.transform.localScale = Vector3.Lerp(alert.transform.localScale, Vector3.one, 3f * Time.deltaTime);
			yield return null;
		}
	}

	private IEnumerator hideAlert()
	{
		while (alert.transform.localScale.magnitude > 1.5f)
		{
			alert.transform.localScale = Vector3.Lerp(alert.transform.localScale, Vector3.one * 0.8f, 10f * Time.deltaTime);
			yield return null;
		}
		alert.SetActive(false);
		GameController.isPaused = false;
	}
}
