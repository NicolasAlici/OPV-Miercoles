using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : CustomMethods
{
    [SerializeField] private ObjectPooler objectPooler;
    [SerializeField] private int boostSpawnBalls = 2;
    private List<CustomGameObject> activeBalls = new List<CustomGameObject>();

    public override void CustomStart()
    {
        SpawnBall();
    }

    public override void CustomUpdate()
    {
        //Pelota perdida (ejemplo)
        if (Input.GetKeyDown(KeyCode.L) && activeBalls.Count > 0)
        {
            CustomGameObject ball = activeBalls[0];
            OnBallLost(ball);
        }
    }

    private void SpawnBall()
    {
        CustomGameObject ball = objectPooler.GetInstanceFromPool();
        ball.transform.position = transform.position;
        ball.SetActive(true);
        activeBalls.Add(ball);
    }

    private void SpawnAdditionalBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            FindObjectOfType<CustomUpdateManager>().methodsList.Add(new CustomGameObject(this.gameObject));
            SpawnBall();
        }
    }

    public void OnBallCollectedBoost()
    {
        SpawnAdditionalBalls(boostSpawnBalls);
    }

    public void OnBallLost(CustomGameObject ball)
    {
        objectPooler.ReturnInstanceToPool(ball);
        activeBalls.Remove(ball);
        //Agregar método para sacar una vida al jugador y chequear para crear una nueva bola si no
    }

}
