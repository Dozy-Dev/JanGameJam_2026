using UnityEngine;

public class EnemyHurtBox : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 5;
    public int CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(DamageInfo dmg)
    {
        CurrentHealth -= dmg.Amount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
