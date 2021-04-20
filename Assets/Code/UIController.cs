using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] private Building building;
    [SerializeField] private Unit unit;
    [SerializeField] private TMP_Text selectedName;
    [SerializeField] private TMP_Text selectedDesc;
    [SerializeField] private SpriteRenderer selectedImage;
    [SerializeField] private RectTransform rect;
    [SerializeField] GameObject content, production;
    public GameObject selectedObject;
    private BuildingController bc;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        BuildingController.onClicked += SetFieldsBuilding;
        UnitController.onClicked += SetFieldsUnit;
        SpawnBuilding.onClicked += CloseRightPanel;
    }

    private void SetFieldsBuilding(Building data, GameObject buildingObject)
    {
        selectedObject = buildingObject;
        bc = selectedObject.GetComponent<BuildingController>();
        building = data;
        content.SetActive(true);
        selectedName.text = building.buildingName;
        selectedImage.sprite = building.buildingImage;
        selectedDesc.text = building.buildingDescription;
        production.SetActive(building.hasProduction);

        //Fix UI Image for different sizes of buildings.
        if (building.buildingName.Equals("Farm"))
        {
            rect.localScale = new Vector3(30, 30, 30);
            rect.localPosition = new Vector3(rect.localPosition.x, 160, rect.localPosition.z);
        }
        else
        {
            rect.localScale = new Vector3(20, 20, 20);
            rect.localPosition = new Vector3(rect.localPosition.x, 200, rect.localPosition.z);
        }
    }

    private void SetFieldsUnit(Unit data, GameObject unitObject)
    {
        if (selectedObject != null && selectedObject.CompareTag(Tags.unit))
        {
            selectedObject.GetComponent<UnitController>().target.SetActive(false);
        }
        selectedObject = unitObject;
        selectedObject.GetComponent<UnitController>().target.SetActive(true);
        unit = data;
        content.SetActive(true);
        selectedName.text = unit.unitName;
        selectedImage.sprite = unit.unitImage;
        selectedDesc.text = unit.unitDescription;
        production.SetActive(false);
    }

    public void CloseRightPanel()
    {
        if (selectedObject != null)
        {
            if (selectedObject.CompareTag(Tags.unit))
            {
                selectedObject.GetComponent<UnitController>().target.SetActive(false);
            }
            selectedObject = null;
        }
        if (content.activeInHierarchy.Equals(true))
        {
            content.SetActive(false);
        }
    }

    public void SpawnSoldier()
    {
        bc.soldier = ObjectPooler.SharedInstance.GetPooledObject(Tags.unit);
        int rnd = Random.Range(0, bc.spawnPoints.Count);
        bc.spawnPos = Vector3Int.FloorToInt(bc.spawnPoints[rnd].transform.position);
        bc.soldier.transform.position = bc.spawnPos;
        bc.soldier.SetActive(true);
        bc.soldier = null;
    }

    public void CheckSpawnPoints()
    {
        foreach (Transform child in selectedObject.transform)
        {
            if (child.GetComponent<spawnPoint>().canSpawn == false)
            {
                if (bc.spawnPoints.Contains(child.gameObject))
                {
                    bc.spawnPoints.Remove(child.gameObject);
                }
            }
            else
            {
                if (!bc.spawnPoints.Contains(child.gameObject))
                {
                    bc.spawnPoints.Add(child.gameObject);
                }
            }
        }

        if (bc.spawnPoints.Count != 0)
        {
            SpawnSoldier();
        }
        else
        {
            Debug.Log("No available space to spawn a unit!");
        }

    }
}
