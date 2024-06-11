using System.Runtime.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CustomMethods
{
    [SerializeField] private float posX = 0f;
    [SerializeField] private float speed = 10f;

    private float dt;
    private Vector2 newPos;
    public override void CustomStart()
    {
        base.CustomStart();
        posX = transform.position.x;
        newPos = transform.position;
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        dt = Time.deltaTime;
        Movement(dt);

        newPos.x = posX;
        newPos.y = transform.position.y;

        transform.position = newPos;
    }

    public void Movement(float delta)
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            posX -= speed * delta;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            posX += speed * delta;
        }
    }
}
