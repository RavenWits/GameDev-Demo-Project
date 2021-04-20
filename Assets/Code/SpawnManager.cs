// SpawnManager
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script calculates and manages spawn point positions according to any size of buildings. The current state is a bit buggy there may be calculation errors. I did not have enough time to finish this script.
/// </summary>

public class SpawnManager : MonoBehaviour
{
	[SerializeField]
	private Building building;

	private int width;

	private int height;

	private int size;

	private float aspectratio;

	private float difference;

	private List<GameObject> points = new List<GameObject>();

	[SerializeField]
	private GameObject[] spawnPoints;

	private GameObject spawnPoint;

	private void Start()
	{
		spawnPoint = new GameObject("spawnpoint", typeof(spawnPoint), typeof(BoxCollider2D), typeof(Rigidbody2D));
		spawnPoint.transform.parent = base.transform;
		spawnPoint.transform.SetScaleX(1f / base.transform.localScale.x);
		spawnPoint.transform.localPosition = new Vector3(1.5f, 1.5f, 0f);
		spawnPoint.tag = "Spawn";
		spawnPoint.GetComponent<BoxCollider2D>().isTrigger = true;
		spawnPoint.GetComponent<BoxCollider2D>().size = Vector2.one * (GameController.instance.gridSize - 0.2f);
		spawnPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
		width = building.Width;
		height = building.Height;
		aspectratio = width / height;
		size = (width + height) * 2 + 4;

		CreateSpawnPoint();
	}

	private void CreateSpawnPoint()
	{
		for (int m = 1; m < size; m++)
		{
			points.Add(Object.Instantiate(spawnPoint, base.transform));
		}
		spawnPoints = points.ToArray();
		for (int l = 0; l < width + 1; l++)
		{
			if (spawnPoints[l].transform.GetSiblingIndex() > 1)
			{
				int prev = spawnPoints[l].transform.GetSiblingIndex() - 2;
				spawnPoints[l].transform.SetPosX(spawnPoints[prev].transform.localPosition.x - 3f);
			}
			else
			{
				spawnPoints[l].transform.SetPosX(spawnPoints[l].transform.localPosition.x - 3f);
			}
		}
		for (int k = width + 1; k < width + height + 2; k++)
		{
			if (spawnPoints[k].transform.GetSiblingIndex() > 1)
			{
				int prev2 = spawnPoints[k].transform.GetSiblingIndex() - 2;
				spawnPoints[k].transform.SetPosX(spawnPoints[k].transform.localPosition.x - (float)(size - 5));
				spawnPoints[k].transform.SetPosY(spawnPoints[prev2].transform.localPosition.y - 3f);
			}
			else
			{
				spawnPoints[k].transform.SetPosY(spawnPoints[k].transform.localPosition.y - 3f);
			}
		}
		for (int j = width + height + 2; j < width * height - 1; j++)
		{
			if (spawnPoints[j].transform.GetSiblingIndex() > 1)
			{
				int prev3 = spawnPoints[j].transform.GetSiblingIndex() - 2;
				spawnPoints[j].transform.SetPosY(spawnPoints[j].transform.localPosition.y - (float)(size - 5));
				spawnPoints[j].transform.SetPosX(spawnPoints[prev3].transform.localPosition.x + 3f);
			}
			else
			{
				spawnPoints[j].transform.SetPosX(spawnPoints[j].transform.localPosition.x + 3f);
			}
		}
		for (int i = width * height - 1; i < size - 1; i++)
		{
			if (spawnPoints[i].transform.GetSiblingIndex() > 1)
			{
				int prev4 = spawnPoints[i].transform.GetSiblingIndex() - 2;
				spawnPoints[i].transform.SetPosY(spawnPoints[prev4].transform.localPosition.y + 3f);
			}
			else
			{
				spawnPoints[i].transform.SetPosY(spawnPoints[i].transform.localPosition.y + 3f);
			}
		}
	}
}
