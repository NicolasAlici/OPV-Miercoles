using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lifes : MonoBehaviour
{
    [SerializeField] Player _player;
    private TMP_Text _lifeText;

    private void Awake()
    {
        _lifeText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        float lives = _player.maxBallsLost - _player.currentBallsLost;
        _lifeText.text = lives.ToString();
    }
}
