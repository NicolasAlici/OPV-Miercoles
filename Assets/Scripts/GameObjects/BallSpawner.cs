using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : CustomMethods
{
    [SerializeField] private ObjectPooler objectPooler;
    [SerializeField] private int boostSpawnBalls = 2;
    private List<Ball> activeBalls = new List<Ball>();
    [SerializeField] private KeyCode launchKey = KeyCode.Space;
    [SerializeField] private Player player;

    public override void CustomAwake()
    {
        base.CustomAwake();
        //player = GetComponent<Player>();
        SpawnBall();
    }

    public override void CustomUpdate()
    {
        // Lanzar las pelotas
        if (Input.GetKeyDown(launchKey))
        {
            foreach (Ball ball in activeBalls)
            {
                ball.Launch();
            }
        }
    }

    private void SpawnBall()
    {
        GameObject ballObject = objectPooler.GetInstanceFromPool();
        ballObject.transform.position = transform.position;
        Ball ball = ballObject.GetComponent<Ball>();
        if (ball != null)
        {
            ballObject.SetActive(true);
            ball.player = player;
            activeBalls.Add(ball);
        }
    }

    private void SpawnAdditionalBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnBall();
        }
    }

    public void OnBallCollectedBoost()
    {
        SpawnAdditionalBalls(boostSpawnBalls);
    }

    public void OnBallLost(GameObject ball)
    {
        Ball ballComponent = ball.GetComponent<Ball>();
        if (ballComponent != null)
        {
            objectPooler.ReturnInstanceToPool(ball);
            activeBalls.Remove(ballComponent);
            Debug.Log("Perdida");
            player.currentBallsLost++;
            if(player != null)
            {
                SpawnBall();
            }
        }
        // Agregar método para sacar una vida al jugador y chequear para crear una nueva bola si no
    }
}
