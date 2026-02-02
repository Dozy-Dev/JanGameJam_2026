using UnityEngine;
using System;

public class WinScreenManager : MonoBehaviour
{
     public GameObject winScreenMenuUI;
    private Action<object> winScreenListener;

    [SerializeField] private SceneController scenecontroller;

    private void Awake()
    {
        winScreenMenuUI.SetActive(false);

        winScreenListener = _ => WinScreenUI();
        GameEventSystem.Instance.RegisterListener(GameEvent.PlayerWon,winScreenListener);
    }
    void WinScreenUI()
    {
        winScreenMenuUI.SetActive(true);
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
