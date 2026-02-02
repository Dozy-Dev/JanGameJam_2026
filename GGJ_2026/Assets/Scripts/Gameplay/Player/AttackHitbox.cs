using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("Tuning")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce = 3f;

    private readonly HashSet<int> hitIdsThisSwing = new();
    private Transform owner;
    private bool facingRight = true;

    public void Arm(Transform ownerTransform, bool facingRightNow, int dmg, float kb)
    {
        owner = ownerTransform;
        facingRight = facingRightNow;
        damage = dmg;
        knockbackForce = kb;
        hitIdsThisSwing.Clear();
        gameObject.SetActive(true);
    }

    public void Disarm()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int id = other.GetInstanceID();
        if (!hitIdsThisSwing.Add(id))
            return;

        if (other.TryGetComponent<IDamageable>(out var dmgTarget))
        {
            Vector2 dir = facingRight ? Vector2.right : Vector2.left;

            dmgTarget.TakeDamage(new DamageInfo
            {
                Amount = damage
            });
        }
    }
}
