// UnitController
using Pathfinding;
using UnityEngine;

public class UnitController : MonoBehaviour
{

	public Unit unit;
	private Vector3Int gridPos;
	[SerializeField] private Animator anim;
	private SpriteRenderer sr;
	public AIPath ai;
	public GameObject target;
	[SerializeField] private SpriteRenderer targetIcon;
	[SerializeField] private Sprite click;
	public int currentHealth;

	public int CurrentHealth
	{
		get
		{
			return CurrentHealth;
		}
		set
		{
			CurrentHealth = unit.health;
		}
	}
	public delegate void ClickAction(Unit data, GameObject unitObject);
	public static event ClickAction onClicked;

	//Creates a target gameobject with texture on unit spawn. And associate it with the A* pathfinding system.
	private void OnEnable()
	{
		currentHealth = unit.health;
		sr = GetComponent<SpriteRenderer>();
		target = new GameObject("Target");
		target.transform.position = base.transform.position;
		target.transform.localScale = Vector2.one * 8f;
		targetIcon = target.AddComponent<SpriteRenderer>();
		targetIcon.sprite = click;
		targetIcon.sortingOrder = 2;
		target.SetActive(false);
		base.gameObject.GetComponent<AIDestinationSetter>().target = target.transform;
	}


	private void Update()
	{
		if (Input.GetMouseButtonDown(1) && UIController.instance.selectedObject != null && UIController.instance.selectedObject.Equals(this.gameObject))
		{
			gridPos = GameController.instance.map.WorldToCell(GameController.instance.truePos);
			if (GameController.instance.map.GetTile(gridPos))
			{
				//Finds center point of the grid and sets target position to the desired grid.
				target.transform.position = GameController.instance.truePos + Vector2Int.one * 6;
			}
		}

		//If unit reached its destination or has no path to follow. Stops its walk animation.
		if (ai.hasPath.Equals(false) || ai.reachedEndOfPath.Equals(true))
		{
			if (anim.GetBool("HasTarget").Equals(true))
			{
				anim.SetBool("HasTarget", false);
			}
		}
		else
		{
			//Makes unit face the direction it goes.
			sr.flipX = ((!(base.transform.position.x <= ai.steeringTarget.x)) ? true : false);

			//Stars walking animation.
			if (anim.GetBool("HasTarget").Equals(false))
			{
				anim.SetBool("HasTarget", true);
			}

		}

	}

	//Fires and event to the subcribers on click.
	private void OnMouseDown()
	{
		if (UnitController.onClicked != null)
		{
			UnitController.onClicked(unit, base.gameObject);
		}
	}
}
