using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGameObject : CustomMethods
{
    private GameObject _gameObject;

    public CustomGameObject(GameObject gameObject)
    {
        _gameObject = gameObject;
    }

    public void SetActive(bool value)
    {
        _gameObject.SetActive(value);
    }

    public bool isActive()
    {
        return _gameObject.activeInHierarchy;
    }

    public void SetParent(Transform parent)
    {
        _gameObject.transform.SetParent(parent);
    }

    public GameObject GetGameObject()
    {
        return _gameObject;
    }
}
