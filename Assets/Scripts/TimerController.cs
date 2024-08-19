using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    [SerializeField]
    float maxTime;

    [SerializeField]
    Image timer;

    private PlayerController2D _player;

    private float _currentTime;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController2D>();
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
            //SceneManager.LoadScene("Win");
            enabled = false;
            return;
        }

        timer.fillAmount = _currentTime / maxTime;
    }
}

