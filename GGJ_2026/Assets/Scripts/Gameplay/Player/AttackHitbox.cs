using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("Shape")]
    [SerializeField] private float attackRadius = 0.9f;
    [SerializeField] private Vector2 localOffset = new Vector2(0.8f, 0f);

    private readonly HashSet<int> hitIdsThisSwing = new();

    private Transform owner;
    private bool facingRight;
    private int damage;
    private bool armed;

    public void Arm(Transform ownerTransform, bool facingRightNow, int dmg, float kb)
    {
        owner = ownerTransform;
        facingRight = facingRightNow;
        damage = dmg;
        hitIdsThisSwing.Clear();
        armed = true;

        CheckHits();
    }

    public void Disarm()
    {
        armed = false;
    }

    private void FixedUpdate()
    {
        if (!armed) return;
        CheckHits();
    }

    private void CheckHits()
    {
        Vector2 hitPoint = (Vector2)owner.position +
                           new Vector2(facingRight ? localOffset.x : -localOffset.x,
                                       localOffset.y);

        var player = FindFirstObjectByType<PlayerHurtbox>();
        if (player == null) return;

        float dist = Vector2.Distance(hitPoint, player.transform.position);

        if (dist <= attackRadius)
        {
            int id = player.GetInstanceID();
            if (!hitIdsThisSwing.Add(id))
                return;

            Vector2 dir = facingRight ? Vector2.right : Vector2.left;

            player.TakeDamage(new DamageInfo
            {
                Amount = damage
            });
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 center = (Vector2)transform.position + localOffset;
        Gizmos.DrawWireSphere(center, attackRadius);
    }
#endif
}
