using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : CustomMethods
{
    [SerializeField] private float posX = 0f;
    [SerializeField] private float posY = 0f;
    public float velX = 0f;
    public float velY = 0f;
    public Player player;
    [SerializeField] private float aceX = 0f;
    [SerializeField] private float aceY = 0f;
    [SerializeField] private float initialVelocityX = 5f;
    [SerializeField] private float initialVelocityY = 5f;
    private bool isMoving = false;
    private Vector2 newPos;
    private float dt;

    public float loseYPosition = -5f;
    private BallSpawner ballSpawner;

    public override void CustomStart()
    {
        base.CustomStart();
        posX = transform.position.x;
        posY = transform.position.y;

        newPos = transform.position;

        ballSpawner = FindAnyObjectByType<BallSpawner>();
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        dt = Time.deltaTime;

        if (isMoving)
        {
            Movement(dt);
        }
        else if (player != null)
        {
            posX = player.transform.position.x;
        }

        newPos.x = posX;
        newPos.y = posY;
        transform.position = newPos;

        CheckIfLost();
    }

    private void CheckIfLost()
    {
        if (posY < loseYPosition)
        {
            ballSpawner.OnBallLost(gameObject);
        }
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

    public void Launch()
    {
        velX = initialVelocityX;
        velY = initialVelocityY;
        isMoving = true;
    }

    public void Stop()
    {
        velX = 0;
        velY = 0;
        isMoving = false;
    }
}
