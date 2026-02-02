using UnityEngine;
using UnityEngine.UI;

public class HealthLogUI : MonoBehaviour
{
    private PlayerStats playerStats;
    
    [SerializeField] private Image[] lifeShrooms;

    [SerializeField] private Sprite[] lifeShroomSprites;

    private const int healthPerShroom = 2;

    private void Awake()
    {
        playerStats = GameObject.FindFirstObjectByType<PlayerStats>();
    }
    private void OnEnable()
    {
        playerStats.OnHealthChanged += UpdateLife;
        UpdateLife(playerStats.CurrentHealth);
    }

    private void OnDisable()
    {
        playerStats.OnHealthChanged -= UpdateLife;
    }

    private void UpdateLife(int currentHealth)
    {
        for (int i = 0; i < lifeShrooms.Length; i++)
        {
            int health = currentHealth - i * healthPerShroom;
            int spriteIndex = Mathf.Clamp(health, 0, healthPerShroom);

            lifeShrooms[i].sprite = lifeShroomSprites[spriteIndex];
        }
    }
}
