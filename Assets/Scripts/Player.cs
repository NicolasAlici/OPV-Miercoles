using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CustomMethods
{
    [SerializeField] private float posX = 0f;
    [SerializeField] private float speed = 0f;
    private float dt;

    public override void CustomStart()
    {
        base.CustomStart();
        posX = transform.position.x;
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        dt = Time.deltaTime;
        Movement(dt);

        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
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
