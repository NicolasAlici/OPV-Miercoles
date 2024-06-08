using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class spriteFromAtlas : CustomMethods
{
    [SerializeField] SpriteAtlas _atlas;
    [SerializeField] string _spriteName;

    public override void CustomStart()
    {
        GetComponent<Image>().sprite = _atlas.GetSprite(_spriteName);
    }
}
