using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksLife : CustomMethods
{
    [SerializeField] private int maxBallHits;
    [SerializeField] private int currentBallHits;

    private ObjectPooler _pool;

    public override void CustomAwake()
    {
        base.CustomAwake();
        _pool = GetComponent<ObjectPooler>();
    }

    public void getHit()
    {
        if(CompareTag("Ball"))
        {
            currentBallHits++;
        }

        if(currentBallHits >= maxBallHits)
        {
            DestroyBrick();
        }
    }

    public void DestroyBrick()
    {
        _pool.ReturnInstanceToPool(this.gameObject);
    }
}
