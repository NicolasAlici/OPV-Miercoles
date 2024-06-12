using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : CustomMethods
{
    [SerializeField] private int maxBallHits;
    [SerializeField] private int currentBallHits;
    [SerializeField] private string poolTag;

    private ObjectPooler _pool;
    private GameManager gameManager;

    public override void CustomAwake()
    {
        base.CustomAwake();
        //_pool = GetComponent<ObjectPooler>();
        _pool = GameObject.FindGameObjectWithTag(poolTag).GetComponent<ObjectPooler>();
        gameManager = GetComponent<GameManager>();
    }

    public override void CustomFixedUpdate()
    {
        base.CustomFixedUpdate();
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"Damaged - {gameObject.name}");
            //GetHit();
        }
    }

    public void GetHit(GameObject brick)
    {
        Bricks brickComponent = brick.GetComponent<Bricks>();
        if (brickComponent != null)
        {
            brickComponent.currentBallHits++;
            if (brickComponent.currentBallHits >= brickComponent.maxBallHits)
            {
                DestroyBrick(brick);
            }
        }
    }

    public void DestroyBrick(GameObject brick)
    {
        //Debug.Log("BRICK " + brick != null);
        //Debug.Log("PILETAAAAAAAAAAAA " + _pool != null);
        _pool.ReturnInstanceToPool(brick);
    }
}