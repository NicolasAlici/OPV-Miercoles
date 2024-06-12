using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : CustomMethods
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 3;

    private List<GameObject> _pool;
    private GameObject _poolContainer;

    public override void CustomAwake()
    {
        _pool = new List<GameObject>();
        _poolContainer = new GameObject($"Pool - {prefab.name}");
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
        newInstance.transform.SetParent(_poolContainer.transform);
        newInstance.SetActive(false);
        
        CustomMethods customMethods = newInstance.GetComponent<CustomMethods>();
        if (customMethods == null)
        {
            customMethods = newInstance.AddComponent<CustomMethods>();
        }
        CustomUpdateManager.Instance.AddToMethodsList(customMethods);

        return newInstance;
    }

    public GameObject GetInstanceFromPool()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _pool[i].SetActive(true);
                CustomMethods customMethods = _pool[i].GetComponent<CustomMethods>();
                if (customMethods == null)
                {
                    customMethods = _pool[i].AddComponent<CustomMethods>();
                }
                CustomUpdateManager.Instance.AddToMethodsList(customMethods);
                return _pool[i];
            }
        }

        // Si no hay instancias disponibles en el pool, crea una nueva
        GameObject newInstance = CreateInstance();
        newInstance.SetActive(true);
        
        _pool.Add(newInstance);
        
        return newInstance;
    }

    public void ReturnInstanceToPool(GameObject instance)
    {
        instance.SetActive(false);
        CustomMethods customMethods = instance.GetComponent<CustomMethods>();
        if (customMethods != null)
        {
            CustomUpdateManager.Instance.RemoveFromMethodsList(customMethods);
        }
    }
}