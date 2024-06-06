using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : CustomMethods
{
    public static GameManager Instance {get; private set;}

    public override void CustomAwake()
    {
        base.CustomAwake();
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public override void CustomStart()
    {
        base.CustomStart();
        Time.timeScale = 1;
    }

}
