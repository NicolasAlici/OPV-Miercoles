using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : CustomMethods
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 3;

    private List<CustomGameObject> _pool;
    private CustomGameObject _poolContainer;
    private CustomUpdateManager _customUpdate;

    public override void CustomAwake()
    {
        _pool = new List<CustomGameObject>();
        _poolContainer = new CustomGameObject(new GameObject($"Pool - {prefab.name}"));
        _customUpdate = FindAnyObjectByType<CustomUpdateManager>();
        CreatePooler();
    }

    private void CreatePooler()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CustomGameObject instance = CreateCustomInstance(prefab, _poolContainer.GetGameObject().transform);
            _pool.Add(instance);
            _customUpdate.methodsList.Add(instance);
        }
    }

    public CustomGameObject GetInstanceFromPool()
    {
         for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].isActive())
            {
                _pool[i].SetActive(true);
                return _pool[i];
            }
        }

        // Si no hay instancias disponibles en el pool, crea una nueva
        CustomGameObject newInstance = CreateCustomInstance(prefab, _poolContainer.GetGameObject().transform);
        newInstance.SetActive(true);
        _pool.Add(newInstance);
        _customUpdate.methodsList.Add(newInstance);

        return newInstance;
    }

    public void ReturnInstanceToPool(CustomGameObject instance)
    {
        instance.SetActive(false);
    }
}
