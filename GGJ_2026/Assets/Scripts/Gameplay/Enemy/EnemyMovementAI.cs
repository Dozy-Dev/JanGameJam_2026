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
    [SerializeField] private float desiredRingMin = 1.5f;
    [SerializeField] private float desiredRingMax = 2.5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float tetherRange = 5f;

    [Header("Move Speeds")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float accelRate = 10f;
    [SerializeField] private float deadZone = 0.05f;

    [Header("Timings or something idk")]
    [SerializeField] private float ticketRequestRetryInterval = 1f;
    [SerializeField] private float maxTimeWithTicket = 10f;

    private Rigidbody2D rb;
    private Guid ticketID;
    private float nextTicketRequestTime = 0f;
    private float holdUntilTime = 0f;
    private bool hasTicket = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        ticketID = Guid.NewGuid();
    }

    private void OnEnable()
    {
        hasTicket = false;
        holdUntilTime = 0f;
    }

    private void OnDisable()
    {
        ReleaseTicket();
    }

    private void OnDestroy()
    {
        ReleaseTicket();
    }

    private void FixedUpdate()
    {
        if (playerTransform == null || AttackTicketManager.Instance == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 toPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = toPlayer.magnitude;

        if(distanceToPlayer > tetherRange)
        {
            //idk just be a zombie or something
            ReleaseTicket();
            rb.linearVelocity = Vector2.zero;
            return;
        }

        AttackSide side = transform.position.x < playerTransform.position.x ? AttackSide.Left : AttackSide.Right;

        if (hasTicket && Time.time > holdUntilTime && distanceToPlayer > attackRange * 0.9f)
        {
            //Still engaging and reserved to fight the player, idk what to do here yet
        }
        else if (hasTicket && Time.time >= holdUntilTime && distanceToPlayer <= attackRange)
        {
            //We're in attack range, hold and then release.
            ReleaseTicket();
        }

        if (!hasTicket && Time.time >= nextTicketRequestTime)
        {
            nextTicketRequestTime = Time.time + ticketRequestRetryInterval;

            if(AttackTicketManager.Instance.TryAcquireTicket(ticketID, side, ticketCostPoints, out var ticket))
            {
                hasTicket = true;
                holdUntilTime = Time.time + maxTimeWithTicket;
            }
        }

        float desiredDistance = hasTicket ? attackRange : RandomRingDistanceStable();

        float delta = distanceToPlayer - desiredDistance;

        Vector2 desiredVel = Vector2.zero;
        if (Mathf.Abs(delta) > deadZone)
        {
            Vector2 dir = toPlayer.normalized;
            desiredVel = dir * Mathf.Sign(delta) * moveSpeed;
        }

        Vector2 v = rb.linearVelocity;
        Vector2 newV = Vector2.MoveTowards(v, desiredVel, accelRate * Time.fixedDeltaTime);
        rb.linearVelocity = newV;
    }

    private float ringNoiseSeed;
    private float RandomRingDistanceStable()
    {
        if (ringNoiseSeed == 0f)
            ringNoiseSeed = UnityEngine.Random.Range(0.1f, 1000f);
        float t = Time.time * 0.9f + ringNoiseSeed;
        float lerp = (Mathf.Sin(t) + 1f) * 0.5f;
        return Mathf.Lerp(desiredRingMin, desiredRingMax, lerp);
    }

    private void ReleaseTicket()
    {
        if (!hasTicket || AttackTicketManager.Instance == null)
            return;

        AttackTicketManager.Instance.ReleaseTicket(ticketID);
        hasTicket = false;
    }
}
