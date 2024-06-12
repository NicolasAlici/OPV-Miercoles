using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : CustomMethods
{
    [SerializeField] private int maxBallHits;
    [SerializeField] private int currentBallHits;
    [SerializeField] private GameObject powerUpPrefab;
    public bool canDropPowerUp;

    private GameManager gameManager;

    public override void CustomAwake()
    {
        base.CustomAwake();
        gameManager = GetComponent<GameManager>();
    }
    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if (Input.GetKeyDown(KeyCode.F))
        {
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
        if (brick == null) return;

        if (canDropPowerUp)
        {
            DropPowerUp(brick.transform.position);
        }

        Destroy(brick);
        if (brick == null)
        {
            gameManager.gameWon();
        }
    }

    private void DropPowerUp(Vector3 position)
    {
        GameObject powerUpObject = Instantiate(powerUpPrefab, position, Quaternion.identity);
        MultiBallPowerUp powerUp = powerUpObject.GetComponent<MultiBallPowerUp>();
        if (powerUp != null)
        {
            powerUp.ActivatePowerUp(position);
            CustomUpdateManager.Instance.AddToMethodsList(powerUp);
        }
    }
}
