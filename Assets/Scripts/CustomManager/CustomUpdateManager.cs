using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUpdateManager : MonoBehaviour
{
    public List<CustomMethods> methodsList;
    public static CustomUpdateManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < methodsList.Count; i++)
        {
            methodsList[i].CustomAwake();
        }
    }

    private void Start()
    {
        for (int i = 0; i < methodsList.Count; i++)
        {
            methodsList[i].CustomStart();
        }
    }

    private void Update()
    {
        for (int i = 0; i < methodsList.Count; i++)
        {
            methodsList[i].CustomUpdate();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < methodsList.Count; i++)
        {
            methodsList[i].CustomFixedUpdate();
        }
    }
    public void AddToMethodsList(CustomMethods customMethods)
    {
        if (!methodsList.Contains(customMethods))
        {
            methodsList.Add(customMethods);
        }
    }
     public void RemoveFromMethodsList(CustomMethods customMethods)
    {
        if (methodsList.Contains(customMethods))
        {
            methodsList.Remove(customMethods);
        }
    }
}
