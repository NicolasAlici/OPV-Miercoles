using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : CustomMethods
{
    [SerializeField] private ObjectPooler objectPooler;
    [SerializeField] private int boostSpawnBalls = 2;
    private List<GameObject> activeBalls = new List<GameObject>();

    public override void CustomStart()
    {
        SpawnBall();
    }

    public override void CustomUpdate()
    {
        //Pelota perdida (ejemplo)
        if (Input.GetKeyDown(KeyCode.L) && activeBalls.Count > 0)
        {
            GameObject ball = activeBalls[0];
            OnBallLost(ball);
        }
    }

    private void SpawnBall()
    {
        GameObject ball = objectPooler.GetInstanceFromPool();
        ball.transform.position = transform.position;
        ball.SetActive(true);
        activeBalls.Add(ball);
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
        objectPooler.ReturnInstanceToPool(ball);
        activeBalls.Remove(ball);
        //Agregar método para sacar una vida al jugador y chequear para crear una nueva bola si no
    }

}