using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovementAI : MonoBehaviour
{
    [Header("Player Target")]
    [SerializeField] private Transform playerTransform;

    [Header("Ticket Movement")]
    [SerializeField] private int ticketCostPoints = 1;

    [Header("Distance From Player")]
    [SerializeField] private float desiredRingMin = 1.8f;
    [SerializeField] private float desiredRingMax = 2.8f;
    [SerializeField] private float attackRange = 1.1f;
    [SerializeField] private float tetherRange = 7f;

    [Header("Move Speeds")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float accelRate = 20f;
    [SerializeField] private float deadZone = 0.05f;

    [Header("Attack")]
    [SerializeField] private AttackHitbox attackHitbox; 
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float knockbackForce = 3f;
    [SerializeField] private float attackWindup = 0.10f;
    private float attackActive = 0.2f;
    [SerializeField] private float attackCooldown = 0.45f;

    [Header("Ticket Timings")]
    [SerializeField] private float ticketRequestRetryInterval = 0.6f;
    [SerializeField] private float maxTimeWithTicket = 4f;

    [Header("Anti-push")]
    [SerializeField] private float minSeparationBuffer = 0.15f; 

    private Rigidbody2D rb;
    private Guid ticketID;

    private float nextTicketRequestTime;
    private float holdUntilTime;

    private bool hasTicket;
    private bool isAttacking;
    private float nextAttackAllowedTime;

    private float ringNoiseSeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        ticketID = Guid.NewGuid();
    }

    private void OnEnable()
    {
        hasTicket = false;
        isAttacking = false;
        nextAttackAllowedTime = 0f;
        holdUntilTime = 0f;
    }

    private void OnDisable() => ReleaseTicket();
    private void OnDestroy() => ReleaseTicket();

    private void FixedUpdate()
    {
        if (playerTransform == null || AttackTicketManager.Instance == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 toPlayer = (Vector2)(playerTransform.position - transform.position);
        float dist = toPlayer.magnitude;

        if (dist > tetherRange)
        {
            ReleaseTicket();
            rb.linearVelocity = Vector2.zero;
            return;
        }

        AttackSide side = transform.position.x < playerTransform.position.x ? AttackSide.Right : AttackSide.Left;
        if (hasTicket && Time.time >= holdUntilTime && !isAttacking)
            ReleaseTicket();

        if (!hasTicket && !isAttacking && Time.time >= nextTicketRequestTime)
        {
            nextTicketRequestTime = Time.time + ticketRequestRetryInterval;

            if (AttackTicketManager.Instance.TryAcquireTicket(ticketID, side, ticketCostPoints, out _))
            {
                hasTicket = true;
                holdUntilTime = Time.time + maxTimeWithTicket;
            }
        }

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float minTouchDist = ComputeMinTouchDistance() + minSeparationBuffer;

        float desiredDist;
        if (hasTicket)
            desiredDist = Mathf.Max(minTouchDist, attackRange);
        else
            desiredDist = Mathf.Max(minTouchDist, RandomRingDistanceStable());

        if (hasTicket && dist <= desiredDist + 0.02f && Time.time >= nextAttackAllowedTime)
        {
            StartCoroutine(DoAttack(side));
            return;
        }

        float delta = dist - desiredDist;

        Vector2 desiredVel = Vector2.zero;
        if (Mathf.Abs(delta) > deadZone)
        {
            Vector2 dir = toPlayer.normalized;

            float sign = Mathf.Sign(delta);
            desiredVel = dir * sign * moveSpeed;

            if (sign < 0f) desiredVel *= 1.2f;
        }

        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, desiredVel, accelRate * Time.fixedDeltaTime);
    }

    private System.Collections.IEnumerator DoAttack(AttackSide side)
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(attackWindup);

        if (attackHitbox != null)
        {
            bool facingRight = side == AttackSide.Left; 
            attackHitbox.Arm(transform, facingRight, attackDamage, knockbackForce);
            yield return new WaitForSeconds(attackActive);
            attackHitbox.Disarm();
        }
        else
        {
            yield return new WaitForSeconds(attackActive);
        }

        nextAttackAllowedTime = Time.time + attackCooldown;

        isAttacking = false;
    }

    private float RandomRingDistanceStable()
    {
        if (ringNoiseSeed == 0f)
            ringNoiseSeed = UnityEngine.Random.Range(0.1f, 1000f);

        float t = Time.time * 0.9f + ringNoiseSeed;
        float lerp = (Mathf.Sin(t) + 1f) * 0.5f;
        return Mathf.Lerp(desiredRingMin, desiredRingMax, lerp);
    }

    private float ComputeMinTouchDistance()
    {
        float a = GetApproxRadius(gameObject);
        float b = playerTransform != null ? GetApproxRadius(playerTransform.gameObject) : 0.5f;
        return a + b;
    }

    private float GetApproxRadius(GameObject go)
    {
        if (go.TryGetComponent<Collider2D>(out var col))
        {
            var bounds = col.bounds;
            return Mathf.Max(bounds.extents.x, bounds.extents.y);
        }
        return 0.5f;
    }

    private void ReleaseTicket()
    {
        if (!hasTicket || AttackTicketManager.Instance == null)
            return;

        AttackTicketManager.Instance.ReleaseTicket(ticketID);
        hasTicket = false;
    }
}
