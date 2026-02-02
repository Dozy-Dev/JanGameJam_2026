using UnityEngine;

public class PlayerHurtbox : MonoBehaviour, IDamageable
{
    [SerializeField] private int iFrameMs = 350;
    private float invulnUnitl;

    public void TakeDamage(DamageInfo dmg)
    {
        if(Time.time < invulnUnitl)
        {
            return;
        }

        invulnUnitl = Time.time + (iFrameMs / 1000f);

        GameEventSystem.Instance.TriggerEvent(GameEvent.PlayerTakeDamage, dmg.Amount);
    }
}
