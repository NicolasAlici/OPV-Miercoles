using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : CustomMethods
{
    [SerializeField] private int maxBallHits;
    [SerializeField] private int currentBallHits;

    private ObjectPooler _pool;
    private CustomGameObject _customGameObject;

    public override void CustomStart()
    {
        base.CustomStart();
        _pool = GetComponent<ObjectPooler>();
        _customGameObject = new CustomGameObject(this.gameObject);
        // FindObjectOfType<CustomUpdateManager>().methodsList.Add(_customGameObject);
    }

    public override void CustomFixedUpdate()
    {
        base.CustomFixedUpdate();
        var ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
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
        _pool.ReturnInstanceToPool(_customGameObject);
    }
}
