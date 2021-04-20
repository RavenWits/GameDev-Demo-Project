// ObjectPooler
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	public static ObjectPooler SharedInstance;

	public List<ObjectPoolItem> itemsToPool;

	public List<GameObject> pooledObjects;

	private void Awake()
	{
		if (SharedInstance == null)
		{
			SharedInstance = this;
		}
	}

	//Initializes Object Pool
	private void Start()
	{
		pooledObjects = new List<GameObject>();
		foreach (ObjectPoolItem item in itemsToPool)
		{
			for (int i = 0; i < item.amountToPool; i++)
			{
				GameObject obj = Object.Instantiate(item.objectToPool);
				obj.SetActive(false);
				pooledObjects.Add(obj);
			}
		}
	}

	//Gets the item with given tag from pool. I`ve used tag because if we use something else it can cause performance issues with a huge pool.
	public GameObject GetPooledObject(string tag)
	{
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
			{
				return pooledObjects[i];
			}
		}
		foreach (ObjectPoolItem item in itemsToPool)
		{
			if (item.objectToPool.tag == tag && item.shouldExpand)
			{
				GameObject obj = Object.Instantiate(item.objectToPool);
				obj.SetActive(false);
				pooledObjects.Add(obj);
				return obj;
			}
		}
		return null;
	}
}

