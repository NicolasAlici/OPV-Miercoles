using System.Runtime.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CustomMethods
{
    [SerializeField] private float posX = 0f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    public int maxBallsLost;
    public int currentBallsLost;

    private float dt;
    private Vector2 newPos;
    private GameManager gameManager;
    public override void CustomStart()
    {
        base.CustomStart();
        gameManager = GetComponent<GameManager>();
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

        if(currentBallsLost >= maxBallsLost)
        {
            gameManager.gameLose();
            gameObject.SetActive(false); 
        }
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

        posX = Mathf.Clamp(posX, minX, maxX);
    }
}
