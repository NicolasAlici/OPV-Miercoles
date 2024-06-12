using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : CustomMethods
{
    [SerializeField] private ObjectPooler objectPooler;
    [SerializeField] private int boostSpawnBalls = 2;
    private List<Ball> activeBalls = new List<Ball>();
    [SerializeField] private KeyCode launchKey = KeyCode.Space;
    public bool launchMultiBall;
    [SerializeField] private Player player;

    public override void CustomAwake()
    {
        base.CustomAwake();
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
        Ball ball = ballObject.GetComponent<Ball>();
        if (ball != null)
        {
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
        if(launchMultiBall == true && activeBalls.Count <= 3 && activeBalls.Count >= 1)
        {
            SpawnAdditionalBalls(boostSpawnBalls);
        }
        
    }

    public void OnBallLost(GameObject ball)
    {
        Ball ballComponent = ball.GetComponent<Ball>();
        if (ballComponent != null)
        {
            objectPooler.ReturnInstanceToPool(ball);
            activeBalls.Remove(ballComponent);
            ballComponent.Stop();

            if (activeBalls.Count <= 0)
            {
                player.currentBallsLost++;
                if (player != null)
                {
                    SpawnBall();
                }
            }
        }
    }
}