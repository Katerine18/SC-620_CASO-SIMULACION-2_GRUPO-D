using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerTopDownController : MonoBehaviour
{
    [SerializeField]
    float maxTime;

    [SerializeField]
    Image timer;

    private PlayerMovementTopDown _player;
    public LevelManager LevelManager;

    private float _currentTime;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovementTopDown>();
    }

    private void Start()
    {
        _currentTime = maxTime;
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0.0F)
        {
            LevelManager.GameWinnerLevel();
            enabled = false;
            return;
        }
        timer.fillAmount = _currentTime / maxTime;
    }
}
