using UnityEngine;
using Pathfinding;


public class UnitController : MonoBehaviour
{
    public Unit unit;
    private Vector3Int gridPos;
    [SerializeField] private Animator anim;
    private SpriteRenderer sr;
    public AIPath ai;
    public GameObject target;
    [SerializeField] SpriteRenderer targetIcon;
    [SerializeField] Sprite click;


    public delegate void ClickAction(Unit data, GameObject unitObject);
    public static event ClickAction onClicked;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        target = new GameObject("Target");
        target.transform.position = transform.position;
        target.transform.localScale = Vector2.one * 8;
        targetIcon = target.AddComponent<SpriteRenderer>();
        targetIcon.sprite = click;
        targetIcon.sortingOrder = 2;
        target.SetActive(false);
        gameObject.GetComponent<AIDestinationSetter>().target = target.transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && UIController.instance.selectedObject != null && UIController.instance.selectedObject.Equals(this.gameObject))
        {
            gridPos = GameController.instance.map.WorldToCell(GameController.instance.truePos);

            if (GameController.instance.map.GetTile(gridPos))
            {
                target.transform.position = GameController.instance.truePos + (Vector2Int.one * 6);
                ai.canMove = true;
            }
        }

        if (Input.touchCount > 0 && UIController.instance.selectedObject != null && UIController.instance.selectedObject.Equals(this.gameObject))
        {
            gridPos = GameController.instance.map.WorldToCell(GameController.instance.truePosMobile);

            if (GameController.instance.map.GetTile(gridPos))
            {
                target.transform.position = GameController.instance.truePosMobile + (Vector2Int.one * 6);
                ai.canMove = true;
            }
        }

        //If an Unit still has a path play walking animation.
        if(ai.hasPath.Equals(false) || ai.reachedEndOfPath.Equals(true))
        {
            if(anim.GetBool("HasTarget").Equals(true))
            {
                anim.SetBool("HasTarget" , false);       
            }
        }
        else
        {
            //Rotate Unit depending on the X axis.
            sr.flipX = (transform.position.x <= ai.steeringTarget.x) ? false : true;

            if(anim.GetBool("HasTarget").Equals(false))
            {
                anim.SetBool("HasTarget" , true);
            }
        }
    }

    private void OnMouseDown()
    {
        if (onClicked != null)
        {
            onClicked(unit, gameObject);
        }
    }




}


