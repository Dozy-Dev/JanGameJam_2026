using UnityEngine;

public class PauseInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEventSystem.Instance.TriggerEvent(GameEvent.Paused);
        }
    }
}
