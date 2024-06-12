using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBallPowerUp : CustomMethods
{
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private float targetYPosition = -5f;

    private bool _isFalling = false;
    private CollisionBricks _collisionBricks;
    private BallSpawner _ballSpawner;

    public override void CustomStart()
    {
        base.CustomStart();
        _collisionBricks = FindObjectOfType<CollisionBricks>();
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if (_isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            if (_collisionBricks != null)
            {
                foreach (var playerCollider in _collisionBricks.noBricks)
                {
                    if (playerCollider.bounds.Intersects(GetComponent<BoxCollider>().bounds))
                    {
                        _collisionBricks.SpawnMultiBalls();
                        Destroy(gameObject);
                    }
                    else
                    {
                        _ballSpawner.launchMultiBall = false;
                    }
                }
            }

            if (transform.position.y <= targetYPosition)
            {
                _isFalling = false;
                CustomUpdateManager.Instance.RemoveFromMethodsList(this);
                Destroy(gameObject);
            }
        }
    }

    public void ActivatePowerUp(Vector3 spawnPosition)
    {
        gameObject.SetActive(true);
        transform.position = spawnPosition;
        _isFalling = true;
    }

    private void OnDestroy()
    {
        CustomUpdateManager.Instance.RemoveFromMethodsList(this);
    }
}
