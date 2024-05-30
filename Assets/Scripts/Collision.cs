using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : CustomMethods
{
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject player;

    private Vector3 point;

    public override void CustomStart()
    {
        base.CustomStart();

        point = new Vector3(0, 0, 0);
    }
    public override void CustomUpdate()
    {
        base.CustomUpdate();

        
    }

    public void RectPoint()
    {

    }

}
