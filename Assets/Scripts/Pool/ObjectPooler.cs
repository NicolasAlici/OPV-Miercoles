using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : CustomMethods
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 3;

    private List<GameObject> _pool;

    public override void CustomAwake()
    {
        _pool = new List<GameObject>();
        CreatePooler();
    }

    private void CreatePooler()
    {
        for (int i = 0; i < poolSize; i++)
        {
            _pool.Add(CreateInstance());
        }
    }

    private GameObject CreateInstance()
    {
        GameObject newInstance = Instantiate(prefab);
        newInstance.SetActive(false);
        return newInstance;
    }
}
