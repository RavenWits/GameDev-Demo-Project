// spawnPoint
using UnityEngine;

/// <summary>
/// This script checks if a spawn point can spawn units in real time.
/// </summary>
public class spawnPoint : MonoBehaviour
{
	public bool canSpawn;

	//For debugging purposes I`ve used gizmos to check spawn point status.
	private void OnDrawGizmos()
	{
		if (canSpawn)
		{
			Gizmos.color = Color.green;
		}
		else
		{
			Gizmos.color = Color.red;
		}
		Gizmos.DrawCube(base.transform.position, Vector3.one * 5f);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Building")) || other.gameObject.CompareTag("Unit"))
		{
			canSpawn = false;
		}
		if (other.gameObject.CompareTag("Water"))
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Water") || other.gameObject.layer.Equals(LayerMask.NameToLayer("Building")) || other.gameObject.CompareTag("Unit"))
		{
			canSpawn = true;
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Building")))
		{
			if (other.gameObject.GetComponent<BuildingController>().isPlaced)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				canSpawn = true;
			}
		}
		if (other.gameObject.CompareTag("Unit"))
		{
			canSpawn = false;
		}
	}
}
