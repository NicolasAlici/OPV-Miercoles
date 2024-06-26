using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionBricks : CustomMethods
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private List<Ball> balls;
    [SerializeField] private List<BoxCollider> ballColliders;
    [SerializeField] private List<BoxCollider> bricks;
    [SerializeField] private List<BoxCollider> noBricks;
    [SerializeField] private List<BoxCollider> bricksToRemove;
    
    private Dictionary<BoxCollider, GameObject> _bricksDictionary;

    private Vector2 firstVector;
    private Vector2 secondVector;
    private Vector2 thirdVector;
    private Vector2 fourthVector;

    //private Vector2 ballPosition;
    //private Vector2 ballVelocity;
    //private Vector2 brickPosition;
    //private Vector2 collisionNormal;
    //private Vector2 reflectedVelocity;

    public override void CustomStart()
    {
        base.CustomStart();
        bricks = new List<BoxCollider>();
        noBricks = new List<BoxCollider>();
        bricksToRemove = new List<BoxCollider>();
        _bricksDictionary = new Dictionary<BoxCollider, GameObject>();
        balls = new List<Ball>();
        ballColliders = new List<BoxCollider>();

        GridManager.gridGenerated += UpdateBricks; // Subscribe to the event
        UpdateBricks(); // Initialize bricks if already present in the scene
        UpdateNoBricks();
        FindBallInstances(); // Find the ball instances in the scene

        for (int i = 0; i < bricks.Count; i++)
        {
            GameObject brickObject = bricks[i].gameObject;
            _bricksDictionary.Add(bricks[i], brickObject);
        }
    }

    private void UpdateBricks()
    {
        bricks.Clear(); // Clear the list before updating
        GameObject[] brickObjects = GameObject.FindGameObjectsWithTag("Brick");

        foreach (GameObject brickObject in brickObjects)
        {
            BoxCollider brickCollider = brickObject.GetComponent<BoxCollider>();
            if (brickCollider != null)
            {
                bricks.Add(brickCollider);
                _bricksDictionary[brickCollider] = brickObject;
            }
        }
    }

    private void UpdateNoBricks()
    {
        noBricks.Clear(); // Clear the list before updating
        GameObject[] noBrickObjects = GameObject.FindGameObjectsWithTag("NoBrick");

        foreach (GameObject noBrickObject in noBrickObjects)
        {
            BoxCollider noBrickCollider = noBrickObject.GetComponent<BoxCollider>();
            if (noBrickCollider != null)
            {
                noBricks.Add(noBrickCollider);
            }
        }
    }

    private void FindBallInstances()
    {
        //Debug.Log("buscando");
        balls.Clear();
        ballColliders.Clear();
        GameObject[] ballObjects = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ballObject in ballObjects)
        {
            Ball ballInstance = ballObject.GetComponent<Ball>();
            BoxCollider ballColliderInstance = ballObject.GetComponent<BoxCollider>();
            if (ballInstance != null && ballColliderInstance != null)
            {
                balls.Add(ballInstance);
                ballColliders.Add(ballColliderInstance);
            }
        }

        if (balls.Count == 0)
        {
            Debug.LogWarning("No ball instances found in the scene!");
        }
    }

    public override void CustomFixedUpdate()
    {
        base.CustomFixedUpdate();

        FindBallInstances();
        if (balls.Count == 0 || ballColliders.Count == 0)
        {
            FindBallInstances();
            if (balls.Count == 0 || ballColliders.Count == 0) return; // Ensure we have valid ball instances
        }

        bricksToRemove.Clear(); // Clear the list before updating

        foreach (BoxCollider brick in bricks)
        {
            foreach (BoxCollider ballCollider in ballColliders)
            {
                Ball ball = balls[ballColliders.IndexOf(ballCollider)];
                if (RectCollision(ball, ballCollider, brick))
                {
                    bricksToRemove.Add(brick);
                }
            }
        }


        foreach (BoxCollider noBrick in noBricks)
        {
            foreach (BoxCollider ballCollider in ballColliders)
            {
                Ball ball = balls[ballColliders.IndexOf(ballCollider)];
                RectCollisionNoBrick(ball, ballCollider, noBrick);
            }
        }

        // Remove bricks that were collided with
        foreach (BoxCollider brick in bricksToRemove)
        {
            bricks.Remove(brick);
            if (_bricksDictionary.TryGetValue(brick, out GameObject brickObject))
            {
                Bricks brickComponent = brickObject.GetComponent<Bricks>();
                if (brickComponent != null)
                {
                    brickComponent.DestroyBrick(brickObject);
                }
            }
        }
    }

    private bool RectCollision(Ball ball, BoxCollider ballCollider, BoxCollider brickCollider)
    {
        if (brickCollider == null) return false;

        if (ballCollider.bounds.max.x >= brickCollider.bounds.min.x &&
            ballCollider.bounds.min.x <= brickCollider.bounds.max.x &&
            ballCollider.bounds.max.y >= brickCollider.bounds.min.y &&
            ballCollider.bounds.min.y <= brickCollider.bounds.max.y)
        {
            //ballPosition = ball.transform.position;
            //brickPosition = brickCollider.transform.position;
            //collisionNormal = Vector2.zero;

            float overlapLeft = ballCollider.bounds.max.x - brickCollider.bounds.min.x;
            float overlapRight = brickCollider.bounds.max.x - ballCollider.bounds.min.x;
            float overlapTop = ballCollider.bounds.max.y - brickCollider.bounds.min.y;
            float overlapBottom = brickCollider.bounds.max.y - ballCollider.bounds.min.y;

            float minOverlap = Mathf.Min(overlapLeft, overlapRight, overlapTop, overlapBottom);


            if (minOverlap == overlapLeft)
            {
                //collisionNormal = Vector2.left;
                ball.velX = -Mathf.Abs(ball.velX);
                firstVector.x = brickCollider.bounds.min.x - ballCollider.bounds.extents.x - 0.01f;
                firstVector.y = ball.transform.position.y;
                ball.transform.position = firstVector;
            }
            else if (minOverlap == overlapRight)
            {
                //collisionNormal = Vector2.right;
                ball.velX = Mathf.Abs(ball.velX);
                secondVector.x = brickCollider.bounds.max.x + ballCollider.bounds.extents.x + 0.01f;
                secondVector.y = ball.transform.position.y;
                ball.transform.position = secondVector;
            }
            else if (minOverlap == overlapTop)
            {
                //collisionNormal = Vector2.down;
                ball.velY = -Mathf.Abs(ball.velY);
                thirdVector.x = ball.transform.position.x;
                thirdVector.y = brickCollider.bounds.min.y - ballCollider.bounds.extents.y - 0.01f;
                ball.transform.position = thirdVector;
            }
            else if (minOverlap == overlapBottom)
            {
                //collisionNormal = Vector2.up;
                ball.velY = Mathf.Abs(ball.velY);
                fourthVector.x = ball.transform.position.x;
                fourthVector.y = brickCollider.bounds.max.y + ballCollider.bounds.extents.y + 0.01f;
                ball.transform.position = fourthVector;
            }

            //Debug.Log(brickCollider.gameObject.name);
            return true; // Return true indicating a collision occurred
        }
        return false; // No collision
    }

    private void RectCollisionNoBrick(Ball ball, BoxCollider ballCollider, BoxCollider noBrickCollider)
    {
        if (noBrickCollider == null) return;

        if (ballCollider.bounds.max.x >= noBrickCollider.bounds.min.x &&
            ballCollider.bounds.min.x <= noBrickCollider.bounds.max.x &&
            ballCollider.bounds.max.y >= noBrickCollider.bounds.min.y &&
            ballCollider.bounds.min.y <= noBrickCollider.bounds.max.y)
        {
            float overlapLeft = ballCollider.bounds.max.x - noBrickCollider.bounds.min.x;
            float overlapRight = noBrickCollider.bounds.max.x - ballCollider.bounds.min.x;
            float overlapTop = ballCollider.bounds.max.y - noBrickCollider.bounds.min.y;
            float overlapBottom = noBrickCollider.bounds.max.y - ballCollider.bounds.min.y;

            float minOverlap = Mathf.Min(overlapLeft, overlapRight, overlapTop, overlapBottom);

            if (minOverlap == overlapLeft)
            {
                //collisionNormal = Vector2.left;
                ball.velX = -Mathf.Abs(ball.velX);
                firstVector.x = noBrickCollider.bounds.min.x - ballCollider.bounds.extents.x - 0.01f;
                firstVector.y = ball.transform.position.y;
                ball.transform.position = firstVector;
            }
            else if (minOverlap == overlapRight)
            {
                //collisionNormal = Vector2.right;
                ball.velX = Mathf.Abs(ball.velX);
                secondVector.x = noBrickCollider.bounds.max.x + ballCollider.bounds.extents.x + 0.01f;
                secondVector.y = ball.transform.position.y;
                ball.transform.position = secondVector;
            }
            else if (minOverlap == overlapTop)
            {
                //collisionNormal = Vector2.down;
                ball.velY = -Mathf.Abs(ball.velY);
                thirdVector.x = ball.transform.position.x;
                thirdVector.y = noBrickCollider.bounds.min.y - ballCollider.bounds.extents.y - 0.01f;
                ball.transform.position = thirdVector;
            }
            else if (minOverlap == overlapBottom)
            {
                //collisionNormal = Vector2.up;
                ball.velY = Mathf.Abs(ball.velY);
                fourthVector.x = ball.transform.position.x;
                fourthVector.y = noBrickCollider.bounds.max.y + ballCollider.bounds.extents.y + 0.01f;
                ball.transform.position = fourthVector;
            }
        }
    }
}

