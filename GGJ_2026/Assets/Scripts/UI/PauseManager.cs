using UnityEngine;
using System;

public class PauseManager : MonoBehaviour
{
    
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    private Action<object> pauseListener;

    [SerializeField] private SceneController scenecontroller;

    private void Awake()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);

        pauseListener = _ => TogglePause();
        GameEventSystem.Instance.RegisterListener(GameEvent.Paused,pauseListener);
    }
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        scenecontroller.Load("MainMenu");
        Debug.Log("Loading menu...");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
