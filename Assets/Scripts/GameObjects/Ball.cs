using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Ball : CustomMethods
{

    [SerializeField] private float posX = 0f;
    [SerializeField] private float posY = 0f;
    public float velX = 0f;
    public float velY = 0f;
    [SerializeField] private float aceX = 0f;
    [SerializeField] private float aceY = 0f;

    private float dt;

    public override void CustomStart()
    {
        base.CustomStart();
        posX = transform.position.x;
        posY = transform.position.y;
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        dt = Time.deltaTime;

        Movement(dt);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }

    public void Movement(float delta)
    {
        velX += aceX * delta;
        velY += aceY * delta;

        posX += velX * delta;
        posY += velY * delta;

        aceX = 0;
        aceY = 0;       
    }
}
