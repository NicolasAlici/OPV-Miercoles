using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : CustomMethods
{
    [SerializeField] private int maxBallHits;
    [SerializeField] private int currentBallHits;

    private ObjectPooler _pool;

    public override void CustomAwake()
    {
        base.CustomAwake();
        _pool = GetComponent<ObjectPooler>();
    }

    public override void CustomFixedUpdate()
    {
        base.CustomFixedUpdate();
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"Damaged - {gameObject.name}");
            getHit();
        }
    }

    public void getHit()
    {
        currentBallHits++;
        if(currentBallHits >= maxBallHits)
        {
            DestroyBrick();
        }
    }

    public void DestroyBrick()
    {
        gameObject.SetActive(false);
        _pool.ReturnInstanceToPool(this.gameObject);
    }
}