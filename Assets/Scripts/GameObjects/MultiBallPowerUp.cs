using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MultiBallPowerUp : CustomMethods
{
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private float targetYPosition = -5f;

    private bool _isFalling = false;

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if (_isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    public void ActivatePowerUp(Vector3 spawnPosition)
    {
        gameObject.SetActive(true);
        transform.position = spawnPosition;
        _isFalling = true;
    }

    public void RemovePowerUp(GameObject powerUpPill)
    {
        MultiBallPowerUp powerUpComponent = powerUpPill.GetComponent<MultiBallPowerUp>();
        if (powerUpComponent.transform.position.y <= targetYPosition)
        {
            powerUpComponent._isFalling = false;
            CustomUpdateManager.Instance.RemoveFromMethodsList(powerUpComponent);
            Destroy(powerUpComponent);
        }
    }
}
