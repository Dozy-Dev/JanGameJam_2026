using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private Collider2D hitCollider;
    [SerializeField] private LayerMask damageMask;

    private readonly HashSet<int> hitIdsThisSwing = new();
    private Transform owner;
    private bool facingRight;
    private int damage;
    private float knockbackForce;
    private bool armed;

    private readonly Collider2D[] results = new Collider2D[16];

    private void Awake()
    {
        if (hitCollider == null) hitCollider = GetComponent<Collider2D>();
        hitCollider.enabled = false;
    }

    public void Arm(Transform ownerTransform, bool facingRightNow, int dmg, float kb)
    {
        owner = ownerTransform;
        facingRight = facingRightNow;
        damage = dmg;
        knockbackForce = kb;

        hitIdsThisSwing.Clear();
        armed = true;
        hitCollider.enabled = true;

        ScanOnce(); 
    }

    public void Disarm()
    {
        armed = false;
        hitCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (!armed) return;
        ScanOnce();
    }

    private void ScanOnce()
    {
        int count = Physics2D.OverlapCollider(hitCollider, new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = damageMask,
            useTriggers = true
        }, results);

        for (int i = 0; i < count; i++)
        {
            var c = results[i];
            if (c == null) continue;

            Debug.Log(c.name);

            int id = c.GetInstanceID();
            if (!hitIdsThisSwing.Add(id)) continue;

            var dmgTarget = c.GetComponentInParent<IDamageable>();
            if (dmgTarget == null) continue;

            Vector2 dir = facingRight ? Vector2.right : Vector2.left;
            dmgTarget.TakeDamage(new DamageInfo
            {
                Amount = damage
            });
        }
    }
}
