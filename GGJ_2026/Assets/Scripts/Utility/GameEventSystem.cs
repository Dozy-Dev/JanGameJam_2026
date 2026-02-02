using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameEvent
{
    PlayerDied,
    PlayerWon,
    PlayerTakeDamage,
    PlayerHeal,
    Paused,
    EnemyDied
}

public class GameEventSystem : MonoBehaviour
{
    // Dictionary to hold multiple events, identified by an enum.
    private Dictionary<GameEvent, Action<object>> events;

    private static GameEventSystem _instance;
    public static GameEventSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find existing instance
                _instance = FindFirstObjectByType<GameEventSystem>();

                // If no instance was found, create one
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("GameEventSystem");
                    _instance = singleton.AddComponent<GameEventSystem>();
                    _instance.events = new Dictionary<GameEvent, Action<object>>();
                }
                else
                {
                    _instance.events = new Dictionary<GameEvent, Action<object>>();
                }
            }
            return _instance;
        }
    }

    // Register a new listener for a specific event.
    public void RegisterListener(GameEvent gameEvent, Action<object> listener)
    {
        if (events.ContainsKey(gameEvent))
        {
            events[gameEvent] += listener;
        }
        else
        {
            events[gameEvent] = listener;
        }
    }

    // Unregister an existing listener from a specific event.
    public void UnregisterListener(GameEvent gameEvent, Action<object> listener)
    {
        if (events.ContainsKey(gameEvent))
        {
            events[gameEvent] -= listener;
        }
    }

    // Trigger a specific event, optionally passing parameters to the listeners.
    public void TriggerEvent(GameEvent gameEvent, object param = null)
    {
        if (events.ContainsKey(gameEvent))
        {
            events[gameEvent]?.Invoke(param);
        }
    }
}