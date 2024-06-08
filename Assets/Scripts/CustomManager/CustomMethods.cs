using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMethods : MonoBehaviour
{
    public virtual void CustomAwake() { }
    public virtual void CustomStart() { }
    public virtual void CustomUpdate() { }
    public virtual void CustomFixedUpdate() { }
    public virtual CustomGameObject CreateCustomInstance(GameObject prefab, Transform parent)
    {
        GameObject newInstance = Instantiate(prefab, parent);
        CustomGameObject customInstance = new CustomGameObject(newInstance);
        customInstance.SetParent(parent);
        customInstance.SetActive(false);
        return customInstance;
    }
}
