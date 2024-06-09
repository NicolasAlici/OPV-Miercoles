using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionBricks : CustomMethods
{
    [SerializeField] private Ball ball;
    [SerializeField] private BoxCollider ballCollider;
    [SerializeField] private List<CustomGameObject> bricks;
    [SerializeField] private List<CustomGameObject> noBricks;
    [SerializeField] private List<CustomGameObject> bricksToRemove;

    private Vector2 firstVector;
    private Vector2 secondVector;
    private Vector2 thirdVector;
    private Vector2 fourthVector;

    

    public override void CustomStart()
    {
        base.CustomStart();
        bricks = new List<CustomGameObject>();
        noBricks = new List<CustomGameObject>();
        GridManager.gridGenerated += UpdateBricks; //Nos suscribimos al evento
        UpdateBricks(); //Inicializamos por si ya hay bricks en escena
        UpdateNoBricks();
    }

    void OnDestroy()
    {
        GridManager.gridGenerated -= UpdateBricks; //Nos desuscribimos del evento al destruir el objeto
    }

    private void UpdateBricks()
    {
        bricks.Clear(); //Limpiamos la lista antes de actualizar
        GameObject[] brickObjects = GameObject.FindGameObjectsWithTag("Brick");

        foreach (GameObject brickObject in brickObjects)
        {
            BoxCollider brickCollider = brickObject.GetComponent<BoxCollider>();
            if (brickCollider != null)
            {
                bricks.Add(new CustomGameObject(brickObject));
            }
        }
    }

    private void UpdateNoBricks()
    {
        noBricks.Clear(); //Limpiamos la lista antes de actualizar
        GameObject[] noBrickObjects = GameObject.FindGameObjectsWithTag("NoBrick");

        foreach (GameObject noBrickObject in noBrickObjects)
        {
            BoxCollider noBrickCollider = noBrickObject.GetComponent<BoxCollider>();
            if (noBrickCollider != null)
            {
                noBricks.Add(new CustomGameObject(noBrickObject));
            }
        }
    }

    public override void CustomFixedUpdate()
    {
        base.CustomFixedUpdate();

        foreach (CustomGameObject brick in bricks)
        {
            if (RectCollision(ball, ballCollider, brick.GetGameObject().GetComponent<BoxCollider>()))
            {
                bricksToRemove.Add(brick);
            }
        }

        // Remove collided bricks from the list
        foreach (CustomGameObject brick in bricksToRemove)
        {
            bricks.Remove(brick);
        }

        foreach (CustomGameObject noBrick in noBricks)
        {
            RectCollision(ball, ballCollider, noBrick.GetGameObject().GetComponent<BoxCollider>());
        }
    }

    public bool RectCollision(Ball ball, BoxCollider ballCollider, BoxCollider brickCollider)
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
                ball.velX = -Mathf.Abs(ball.velX);
                firstVector.x = brickCollider.bounds.min.x - ballCollider.bounds.extents.x - 0.01f; 
                firstVector.y = ball.transform.position.y;
                ball.transform.position = firstVector;
            }
            else if (minOverlap == overlapRight)
            {
                ball.velX = Mathf.Abs(ball.velX);
                secondVector.x = brickCollider.bounds.max.x + ballCollider.bounds.extents.x + 0.01f; 
                secondVector.y = ball.transform.position.y;
                ball.transform.position = secondVector;
            }
            else if (minOverlap == overlapTop)
            {
                ball.velY = -Mathf.Abs(ball.velY);
                thirdVector.x = ball.transform.position.x;
                thirdVector.y = brickCollider.bounds.min.y - ballCollider.bounds.extents.y - 0.01f; 
                ball.transform.position = thirdVector;
            }
            else if (minOverlap == overlapBottom)
            {
                ball.velY = Mathf.Abs(ball.velY);
                fourthVector.x = ball.transform.position.x;
                fourthVector.y = brickCollider.bounds.max.y + ballCollider.bounds.extents.y + 0.01f; 
                ball.transform.position = fourthVector;
            }

            return true;
        }

        return false;
    }
}
