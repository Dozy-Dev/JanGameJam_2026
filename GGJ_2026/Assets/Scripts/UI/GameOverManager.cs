using UnityEngine;
using System;

public class GameOverManager : MonoBehaviour
{
    
    public static bool isGameOver = false;
    public GameObject gameOverMenuUI;
    private Action<object> gameOverListener;

    [SerializeField] private SceneController scenecontroller;

    private void Awake()
    {
        isGameOver = false;
        gameOverMenuUI.SetActive(false);

        gameOverListener = _ => GameOverUI();
        GameEventSystem.Instance.RegisterListener(GameEvent.PlayerDied,gameOverListener);
    }
    void GameOverUI()
    {
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
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
