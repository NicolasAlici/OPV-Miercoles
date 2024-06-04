using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collision : CustomMethods
{
    [SerializeField] private Ball ball;
    [SerializeField] private BoxCollider ballCollider;
    [SerializeField] private BoxCollider brick;

    public override void CustomFixedUpdate()
    {
        base.CustomFixedUpdate();

        RectCollision(ball, ballCollider, brick);
    }

    public void RectCollision(Ball ball, BoxCollider ballCollider, BoxCollider brick)
    {
        if (ballCollider.bounds.max.x >= brick.bounds.min.x &&
            ballCollider.bounds.min.x <= brick.bounds.max.x &&
            ballCollider.bounds.max.y >= brick.bounds.min.y &&
            ballCollider.bounds.min.y <= brick.bounds.max.y)
        {
            float overlapLeft = ballCollider.bounds.max.x - brick.bounds.min.x;
            float overlapRight = brick.bounds.max.x - ballCollider.bounds.min.x;
            float overlapTop = ballCollider.bounds.max.y - brick.bounds.min.y;
            float overlapBottom = brick.bounds.max.y - ballCollider.bounds.min.y;

            float minOverlap = Mathf.Min(overlapLeft, overlapRight, overlapTop, overlapBottom);
         
            if (minOverlap == overlapLeft)
            {
                ball.velX = -Mathf.Abs(ball.velX);
                ball.transform.position = new Vector2(brick.bounds.min.x - ballCollider.bounds.extents.x, ball.transform.position.y);
            }
            else if (minOverlap == overlapRight)
            {
                ball.velX = Mathf.Abs(ball.velX);
                ball.transform.position = new Vector2(brick.bounds.max.x + ballCollider.bounds.extents.x, ball.transform.position.y);
            }
            else if (minOverlap == overlapTop)
            {
                ball.velY = -Mathf.Abs(ball.velY);
                ball.transform.position = new Vector2(ball.transform.position.x, brick.bounds.min.y - ballCollider.bounds.extents.y);
            }
            else if (minOverlap == overlapBottom)
            {
                ball.velY = Mathf.Abs(ball.velY);
                ball.transform.position = new Vector2(ball.transform.position.x, brick.bounds.max.y + ballCollider.bounds.extents.y);
            }
        }
    }
}
