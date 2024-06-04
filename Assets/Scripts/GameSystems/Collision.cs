using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collision : CustomMethods
{
    [SerializeField] private Ball ball;
    [SerializeField] private BoxCollider ballCollider;
    [SerializeField] private BoxCollider brick;

    public override void CustomStart()
    {
        base.CustomStart();

        
    }
    public override void CustomUpdate()
    {
        base.CustomUpdate();

        RectCollision(ball, ballCollider, brick);
    }

    public void RectCollision(Ball ball, BoxCollider ballCollider, BoxCollider brick)
    {
        if (ballCollider.bounds.max.x >= brick.bounds.min.x &&
            ballCollider.bounds.min.x <= brick.bounds.max.x &&
            ballCollider.bounds.max.y >= brick.bounds.min.y &&
            ballCollider.bounds.min.y <= brick.bounds.max.y)
        {
            Debug.Log("IF 1");
            ball.velX = Mathf.Abs(ball.velX);
            ball.velY = -Mathf.Abs(ball.velY);
        }            
        
        //if (ballCollider.bounds.min.x <= brick.bounds.max.x) 
        //{
        //    Debug.Log("IF 2");
        //    ball.velX = Mathf.Abs(ball.velX);
        //}
            

        //if (ballCollider.bounds.max.y >= brick.bounds.min.y)
        //{
        //    Debug.Log("IF 3");
        //    ball.velY = -Mathf.Abs(ball.velY);
        //}
        
        //if (ballCollider.bounds.min.y <= brick.bounds.max.y)
        //{
        //    Debug.Log("IF 4");
        //    ball.velY = Mathf.Abs(ball.velY);
        //}
    }
}
