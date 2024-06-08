using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : CustomMethods
{
    public ObjectPool brickPool;
    public int rows = 5;
    public int columns = 10;
    public float brickSpacing = 1.1f;

    public override void CustomStart()
    {
        base.CustomStart();

        // Positioning for spawning bricks
        Vector2 startPos = new Vector2(-columns / 2f, 4f); // Adjust start position as needed

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector2 pos = startPos + new Vector2(col * brickSpacing, -row * brickSpacing);
                GameObject brick = brickPool.GetObject();
                brick.transform.position = pos;
            }
        }
    }

    public void ReturnBrick(GameObject brick)
    {
        brickPool.ReturnObject(brick);
    }
}
