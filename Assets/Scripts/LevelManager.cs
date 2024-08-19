using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadLevel(int levelNo)
    {
        SceneManager.LoadScene(levelNo);
    }

    public void WelcomeLevel()
    {
        LoadLevel(0);
    }

    public void Play()
    {
        SceneManager.LoadScene("PLATFORMER");
    }
    public void Next()
    {
        SceneManager.LoadScene("TOP DOWN");
    }

    public void GameWinnerLevel()
    {
        LoadLevel(3);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
