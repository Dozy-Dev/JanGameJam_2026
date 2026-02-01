using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 10;
    public int lives = 5;

    public int CurrentHealth { get; private set; }

    public event Action OnPlayerDied;
    public event Action<int> OnHealthChanged;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        GameEventSystem.Instance.RegisterListener(GameEvent.PlayerTakeDamage,obj => TakeDamage((int) obj));
        GameEventSystem.Instance.RegisterListener(GameEvent.PlayerHeal,obj => Heal((int) obj));
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
        }

        OnHealthChanged?.Invoke(CurrentHealth);
    }

    private void HandleDeath()
    {
        lives--;

        if (lives <= 0)
        {
            OnPlayerDied?.Invoke();
        }
        else
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth);

        // Move to spawn point?
    }
}
