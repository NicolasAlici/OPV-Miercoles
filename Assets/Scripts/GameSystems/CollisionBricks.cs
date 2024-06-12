using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionBricks : CustomMethods
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private List<Ball> balls;
    [SerializeField] private List<MultiBallPowerUp> powerUp;
    [SerializeField] private List<BoxCollider> ballColliders;
    [SerializeField] private List<BoxCollider> bricks;
    public List<BoxCollider> noBricks;
    [SerializeField] private List<BoxCollider> bricksToRemove;
    [SerializeField] private List<BoxCollider> powerUpCollider;
    [SerializeField] private List<BoxCollider> powerUpToRemove;
    [SerializeField] private BoxCollider player;   
    private Dictionary<BoxCollider, GameObject> _bricksDictionary;
    private Dictionary<BoxCollider, GameObject> _powerUpDictionary;

    private Vector2 firstVector;
    private Vector2 secondVector;
    private Vector2 thirdVector;
    private Vector2 fourthVector;

    public override void CustomStart()
    {
        base.CustomStart();
        bricks = new List<BoxCollider>();
        noBricks = new List<BoxCollider>();
        bricksToRemove = new List<BoxCollider>();
        powerUpCollider = new List<BoxCollider>();
        powerUpToRemove = new List<BoxCollider>();
        _bricksDictionary = new Dictionary<BoxCollider, GameObject>();
        _powerUpDictionary = new Dictionary<BoxCollider, GameObject>();
        balls = new List<Ball>();
        powerUp = new List<MultiBallPowerUp>();
        ballColliders = new List<BoxCollider>();

        GridManager.gridGenerated += UpdateBricks;
        UpdateBricks();
        UpdateNoBricks();
        FindBallInstances();
        FindPowerUpInstances();

        for (int i = 0; i < bricks.Count; i++)
        {
            GameObject brickObject = bricks[i].gameObject;
            _bricksDictionary.Add(bricks[i], brickObject);
        }

        for (int i = 0; i < powerUpCollider.Count; i++)
        {
            GameObject powerUpObject = powerUpCollider[i].gameObject;
            _powerUpDictionary.Add(powerUpCollider[i], powerUpObject);
        }
    }

    private void UpdateBricks()
    {
        bricks.Clear();
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
        noBricks.Clear();
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
    }

    private void FindPowerUpInstances()
    {
        powerUp.Clear();
        powerUpCollider.Clear();
        GameObject[] powerUpObjects = GameObject.FindGameObjectsWithTag("PowerUp");

        foreach (GameObject powerUpObject in powerUpObjects)
        {
            MultiBallPowerUp powerUpInstance = powerUpObject.GetComponent<MultiBallPowerUp>();
            BoxCollider powerUpColliderInstance = powerUpObject.GetComponent<BoxCollider>();
            if (powerUpInstance != null && powerUpColliderInstance != null)
            {
                powerUp.Add(powerUpInstance);
                powerUpCollider.Add(powerUpColliderInstance);
            }
        }
    }

    public override void CustomFixedUpdate()
    {
        base.CustomFixedUpdate();

        FindBallInstances();
        if (balls.Count == 0 || ballColliders.Count == 0)
        {
            FindBallInstances();
            if (balls.Count == 0 || ballColliders.Count == 0) return;
        }

        FindPowerUpInstances();
        if (powerUp.Count == 0 || powerUpCollider.Count == 0)
        {
            FindBallInstances();
            if (powerUp.Count == 0 || powerUpCollider.Count == 0) return;
        }

        bricksToRemove.Clear();
        powerUpToRemove.Clear();

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

        
        foreach (BoxCollider powerUpCol in powerUpCollider)
        {
            if (RectCollisionPowerUp(powerUpCol, player))
            {
                powerUpToRemove.Add(powerUpCol);
            }
        }

        foreach (BoxCollider powerUpCol in powerUpToRemove)
        {
            powerUpCollider.Remove(powerUpCol);
            if (_powerUpDictionary.TryGetValue(powerUpCol, out GameObject powerUpObject))
            {
                MultiBallPowerUp powerUpComponent = powerUpObject.GetComponent<MultiBallPowerUp>();
                if (powerUpComponent != null)
                {
                    powerUpComponent.RemovePowerUp(powerUpObject);
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

            return true;
        }
        return false;
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

    private bool RectCollisionPowerUp(BoxCollider powerUpCollider, BoxCollider playerCollider)
    {
        if (powerUpCollider == null) return false;

        if (playerCollider.bounds.max.x >= powerUpCollider.bounds.min.x &&
            playerCollider.bounds.min.x <= powerUpCollider.bounds.max.x &&
            playerCollider.bounds.max.y >= powerUpCollider.bounds.min.y &&
            playerCollider.bounds.min.y <= powerUpCollider.bounds.max.y)
        {
            return true;
        }
        return false;
    }
}

