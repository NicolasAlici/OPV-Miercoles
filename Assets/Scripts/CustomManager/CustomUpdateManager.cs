using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUpdateManager : MonoBehaviour
{
    public List<CustomMethods> methodsList;

    private void Awake()
    {
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
}
