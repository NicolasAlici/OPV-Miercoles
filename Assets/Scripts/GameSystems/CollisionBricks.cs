using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionBricks : CustomMethods
{
    [SerializeField] private Ball ball;
    [SerializeField] private BoxCollider ballCollider;
    [SerializeField] private List<BoxCollider> bricks;
    [SerializeField] private List<BoxCollider> noBricks;

    private Vector2 firstVector;
    private Vector2 secondVector;
    private Vector2 thirdVector;
    private Vector2 fourthVector;

    public override void CustomStart()
    {
        base.CustomStart();
        bricks = new List<BoxCollider>();
        noBricks = new List<BoxCollider>();
        GridManager.gridGenerated += UpdateBricks; // Nos suscribimos al evento
        UpdateBricks(); // Inicializamos por si ya hay bricks en escena
        UpdateNoBricks();
    }

    void OnDestroy()
    {
        GridManager.gridGenerated -= UpdateBricks; // Nos desuscribimos del evento al destruir el objeto
    }

    private void UpdateBricks()
    {
        bricks.Clear(); // Limpiamos la lista antes de actualizar
        GameObject[] brickObjects = GameObject.FindGameObjectsWithTag("Brick");

        foreach (GameObject brickObject in brickObjects)
        {
            BoxCollider brickCollider = brickObject.GetComponent<BoxCollider>();
            if (brickCollider != null)
            {
                bricks.Add(brickCollider);
            }
        }
    }

    private void UpdateNoBricks()
    {
        noBricks.Clear(); // Limpiamos la lista antes de actualizar
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

    public override void CustomFixedUpdate()
    {
        base.CustomFixedUpdate();

        foreach (BoxCollider brick in bricks)
        {
            RectCollision(ball, ballCollider, brick);
        }

        foreach (BoxCollider noBrick in noBricks)
        {
            RectCollision(ball, ballCollider, noBrick);
        }
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
                firstVector.x = brick.bounds.min.x - ballCollider.bounds.extents.x;
                firstVector.y = ball.transform.position.y;
                ball.transform.position = firstVector;
            }
            else if (minOverlap == overlapRight)
            {
                ball.velX = Mathf.Abs(ball.velX);
                secondVector.x = brick.bounds.max.x + ballCollider.bounds.extents.x;
                secondVector.y = ball.transform.position.y;
                ball.transform.position = secondVector;
            }
            else if (minOverlap == overlapTop)
            {
                ball.velY = -Mathf.Abs(ball.velY);
                thirdVector.x = ball.transform.position.x;
                thirdVector.y = brick.bounds.min.y - ballCollider.bounds.extents.y;
                ball.transform.position = thirdVector;
            }
            else if (minOverlap == overlapBottom)
            {
                ball.velY = Mathf.Abs(ball.velY);
                fourthVector.x = ball.transform.position.x;
                fourthVector.y = brick.bounds.max.y + ballCollider.bounds.extents.y;
                ball.transform.position = fourthVector;
            }

            if (brick.CompareTag("Brick"))
                Destroy(brick.gameObject);
        }
    }
}
