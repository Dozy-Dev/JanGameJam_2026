using UnityEngine;
using System;

public class PauseMenuUi : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    private Action<object> pauseListener;

    private void OnAwake()
    {
        GameEventSystem.Instance.RegisterListener(GameEvent.Paused,pauseListener);
    }

}
