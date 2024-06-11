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

    private List<BoxCollider> bricksToRemove;
    private ObjectPooler _pool;

    private Vector2 firstVector;
    private Vector2 secondVector;
    private Vector2 thirdVector;
    private Vector2 fourthVector;

    private Dictionary<BoxCollider, GameObject> _bricksDictionary;

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

    void OnDestroy()
    {
        GridManager.gridGenerated -= UpdateBricks; // Unsubscribe from the event
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

        if (balls.Count == 0 || ballColliders.Count == 0)
        {
            FindBallInstances();
            if (balls.Count == 0 || ballColliders.Count == 0) return; // Ensure we have valid ball instances
        }

        bricksToRemove.Clear(); // Clear the list before updating

        for (int i = bricks.Count - 1; i >= 0; i--)
        {
            for (int j = ballColliders.Count - 1; j >= 0; j--)
            {
                Ball ball = balls[ballColliders.IndexOf(ballColliders[j])];
                BoxCollider collidedBrick = RectCollision(ball, ballColliders[j], bricks[i]);
                if (collidedBrick != null)
                {
                    GameObject brickObject = collidedBrick.gameObject;
                    if (_bricksDictionary.TryGetValue(collidedBrick, out brickObject))
                    {
                        Bricks brickComponent = brickObject.GetComponent<Bricks>();
                        if (brickComponent != null)
                        {
                            brickComponent.GetHit(brickObject);
                        }
                    }
                    //bricksToRemove.Add(collidedBrick);
                }
            }
            
        } 


        foreach (BoxCollider noBrick in noBricks)
        {
            foreach (BoxCollider ballCollider in ballColliders)
            {
                Ball ball = balls[ballColliders.IndexOf(ballCollider)];
                RectCollision(ball, ballCollider, noBrick);
            }
        }

        // Remove bricks that were collided with
        //foreach (BoxCollider brick in bricksToRemove)
        //{
        //    bricks.Remove(brick);
        //    Destroy(brick.gameObject);
        //}
    }

    public BoxCollider RectCollision(Ball ball, BoxCollider ballCollider, BoxCollider brickCollider)
    {
        if (brickCollider == null) return null;

        if (ballCollider.bounds.max.x >= brickCollider.bounds.min.x &&
            ballCollider.bounds.min.x <= brickCollider.bounds.max.x &&
            ballCollider.bounds.max.y >= brickCollider.bounds.min.y &&
            ballCollider.bounds.min.y <= brickCollider.bounds.max.y)
        {
            float overlapLeft = ballCollider.bounds.max.x - brickCollider.bounds.min.x;
            float overlapRight = brickCollider.bounds.max.x - ballCollider.bounds.min.x;
            float overlapTop = ballCollider.bounds.max.y - brickCollider.bounds.min.y;
            float overlapBottom = brickCollider.bounds.max.y - ballCollider.bounds.min.y;

            float minOverlap = Mathf.Min(overlapLeft, overlapRight, overlapTop, overlapBottom);

            if (minOverlap == overlapLeft)
            {
                ball.velX = -Mathf.Abs(ball.velX);
                firstVector.x = brickCollider.bounds.min.x - ballCollider.bounds.extents.x;
                firstVector.y = ball.transform.position.y;
                ball.transform.position = firstVector;
            }
            else if (minOverlap == overlapRight)
            {
                ball.velX = Mathf.Abs(ball.velX);
                secondVector.x = brickCollider.bounds.max.x + ballCollider.bounds.extents.x;
                secondVector.y = ball.transform.position.y;
                ball.transform.position = secondVector;
            }
            else if (minOverlap == overlapTop)
            {
                ball.velY = -Mathf.Abs(ball.velY);
                thirdVector.x = ball.transform.position.x;
                thirdVector.y = brickCollider.bounds.min.y - ballCollider.bounds.extents.y;
                ball.transform.position = thirdVector;
            }
            else if (minOverlap == overlapBottom)
            {
                ball.velY = Mathf.Abs(ball.velY);
                fourthVector.x = ball.transform.position.x;
                fourthVector.y = brickCollider.bounds.max.y + ballCollider.bounds.extents.y;
                ball.transform.position = fourthVector;
            }
            Debug.Log(brickCollider.gameObject.name);
            return brickCollider; // Return the collided brick
        }
        return null; // No collision
    }
}
