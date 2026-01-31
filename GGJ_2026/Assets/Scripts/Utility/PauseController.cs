using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] bool isPaused = false;
    private int bufferFrames = 20;
    [SerializeField] GameObject PausedCanvas;

    [SerializeField] private InputActionReference escapeAction;


    private void OnEnable()
    {
        if (escapeAction != null)
            escapeAction.action.Enable();
    }

    private void OnDisable()
    {
        if (escapeAction != null)
            escapeAction.action.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        escapeAction.ToInputAction().performed += ctx => TogglePause();
    }

    // Update is called once per frame
    void Update()
    {
        if(bufferFrames > 0)
        {
            bufferFrames--;
        }
    }

    public void TogglePause()
    {
        if (bufferFrames <= 0)
        {
            isPaused = !isPaused;

            if(isPaused)
            {
                Time.timeScale = 0;
                PausedCanvas.SetActive(true);
                GameEventSystem.Instance.TriggerEvent(GameEvent.Paused, true);
            } else
            {
                Time.timeScale = 1;
                PausedCanvas.SetActive(false);
                GameEventSystem.Instance.TriggerEvent(GameEvent.Paused, false);
            }
        }
    }
}
