using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class spriteFromAtlas : MonoBehaviour
{
    [SerializeField] SpriteAtlas _atlas;
    [SerializeField] string _spriteName;

    void Start()
    {
        GetComponent<Image>().sprite = _atlas.GetSprite(_spriteName);
    }
}
