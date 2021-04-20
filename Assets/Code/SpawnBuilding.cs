// SpawnBuilding
using System.Collections;
using TMPro;
using UnityEngine;

public class SpawnBuilding : MonoBehaviour
{
	[SerializeField] private GameObject poolObject;
	[SerializeField] private TMP_Text gridSize;
	private RectTransform parent;
	public string tagName;
	public delegate void ClickAction(GameObject gameObject);
	public static event ClickAction onClicked;

	private void Start()
	{
		tagName = gameObject.tag;
	}

	//Gets object from pool on click and shows its occupaid grid size while disabling other buildings to prevent stacking buildings on top of each other. Then fires the event to subscribers.
	public void selectBuilding()
	{
		poolObject = ObjectPooler.SharedInstance.GetPooledObject(tagName);
		StartCoroutine(ShowGridSize());
		poolObject.transform.position = transform.position;
		poolObject.SetActive(true);
		UIController.instance.DisableBuildingButtons();

		if (SpawnBuilding.onClicked != null && GameController.isPaused.Equals(false))
		{
			SpawnBuilding.onClicked(null);
		}
	}

	//This coroutine triggers the a UI panel animation to show building grid size while player hovers a building on gameboard.
	//I`ve only used this coroutine. Because it is asked in project documentation. 
	private IEnumerator ShowGridSize()
	{
		if (poolObject != null)
		{
			string w = poolObject.GetComponent<BuildingController>().building.Width.ToString();
			string h = poolObject.GetComponent<BuildingController>().building.Height.ToString();
			gridSize.text = "Grid Size: " + w + " x " + h;
			parent = gridSize.transform.parent.GetComponent<RectTransform>();
			Vector3 visiblePosition = new Vector2(100f, 30f);
			Vector3 hiddenPosition = new Vector2(-500f, 30f);
			while (Vector2.Distance(parent.anchoredPosition, visiblePosition) > 2f)
			{
				parent.anchoredPosition = Vector2.Lerp(parent.anchoredPosition, visiblePosition, 12f * Time.deltaTime);
				yield return null;
			}
			yield return new WaitUntil(() => poolObject.GetComponent<BuildingController>().isPlaced);
			while (Vector2.Distance(parent.anchoredPosition, hiddenPosition) > 2f)
			{
				parent.anchoredPosition = Vector2.Lerp(parent.anchoredPosition, hiddenPosition, 16f * Time.deltaTime);
				yield return null;
			}
		}
	}
}
