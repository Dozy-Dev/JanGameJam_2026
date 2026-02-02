using UnityEngine;

public struct DamageInfo
{
    public int Amount;
}

public interface IDamageable
{
    void TakeDamage(DamageInfo dmg);
}