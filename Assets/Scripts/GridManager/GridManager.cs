using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : CustomMethods
{
    public List<GameObject> cubePrefabs;
    public int rows = 6;
    public int columns = 3;
    public float spacingX = 1.8f; 
    public float spacingY = 1.1f;

    public static event Action gridGenerated;

    public override void CustomStart()
    {
        base.CustomStart();
        CreateGrid();
        gridGenerated?.Invoke();
    }

    void CreateGrid()
    {
         Vector3 startPosition = new Vector3(-rows / 2.0f * spacingX, -columns / 2.0f * spacingY, 0);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = startPosition + new Vector3(i * spacingX, j * spacingY, 0);
                GameObject cubePrefab = cubePrefabs[UnityEngine.Random.Range(0, cubePrefabs.Count)];
                Instantiate(cubePrefab, position, Quaternion.identity);
            }
        }
    }
}

