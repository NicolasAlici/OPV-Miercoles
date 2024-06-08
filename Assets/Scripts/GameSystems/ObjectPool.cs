using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : CustomMethods
{
    public GameObject prefab; // The prefab to pool
    public int initialPoolSize = 20; // Initial pool size

    private List<GameObject> pool;

    public override void CustomStart()
    {
        base.CustomStart();
        // Initialize the pool
        pool = new List<GameObject>();

        // Populate the pool
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // If no inactive objects are available, create a new one
        return CreateNewObject();
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
