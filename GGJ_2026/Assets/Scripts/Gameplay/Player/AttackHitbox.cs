using System.Collections.Generic;
using UnityEngine;

public enum Faction { Player, Enemy }

public class AttackHitbox : MonoBehaviour
{
    [Header("Shape")]
    [SerializeField] private float attackRadius = 0.9f;
    [SerializeField] private Vector2 localOffset = new Vector2(0.8f, 0f);

    [Header("Who owns this?")]
    [SerializeField] private Faction ownerFaction = Faction.Player;

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

    public void Disarm() => armed = false;

    private void FixedUpdate()
    {
        if (!armed) return;
        CheckHits();
    }

    private void CheckHits()
    {
        if (owner == null || CombatRegistry.Instance == null) return;

        Vector2 hitPoint = (Vector2)owner.position +
                           new Vector2(facingRight ? localOffset.x : -localOffset.x, localOffset.y);

        IReadOnlyList<IDamageable> targets =
            ownerFaction == Faction.Player
                ? CombatRegistry.Instance.Enemies
                : CombatRegistry.Instance.Players;

        for (int i = 0; i < targets.Count; i++)
        {
            var t = targets[i];
            if (t == null) continue;

            var mb = t as MonoBehaviour;
            if (mb != null && (mb.transform == owner || mb.transform.IsChildOf(owner)))
                continue;

            Vector2 targetPos = mb != null ? (Vector2)mb.transform.position : Vector2.zero;
            float dist = Vector2.Distance(hitPoint, targetPos);

            if (dist > attackRadius)
                continue;

            int id = mb != null ? mb.GetInstanceID() : t.GetHashCode();
            if (!hitIdsThisSwing.Add(id))
                continue;

            Vector2 dir = facingRight ? Vector2.right : Vector2.left;

            t.TakeDamage(new DamageInfo
            {
                Amount = damage
            });
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = ownerFaction == Faction.Player ? Color.green : Color.red;
        Vector2 center = (Vector2)transform.position + localOffset;
        Gizmos.DrawWireSphere(center, attackRadius);
    }
#endif
}
