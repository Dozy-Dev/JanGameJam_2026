using UnityEngine;
using System;
using System.Collections.Generic;

public enum AttackSide { Left, Right }

public class AttackTicketManager : MonoBehaviour
{
    public static AttackTicketManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find existing instance
                _instance = FindFirstObjectByType<AttackTicketManager>();

                // If no instance was found, create one
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("AttackTicketManager");
                    _instance = singleton.AddComponent<AttackTicketManager>();
                }
            }
            return _instance;
        }
    }

    private static AttackTicketManager _instance;


    [Header("Side Capacity, in points")]
    [SerializeField] private int leftSideCapacityPoints = 2;
    [SerializeField] private int rightSideCapacityPoints = 2;

    private readonly Dictionary<Guid, Ticket> activeTickets = new();
    private int leftUsedPoints, rightUsedPoints = 0;

    public bool TryAcquireTicket(Guid ticketID, AttackSide side, int cost, out Ticket ticket)
    {
        ticket = default;
        if (activeTickets.TryGetValue(ticketID, out var existingReserver))
        {
            ticket = existingReserver;
            return true; 
        }

        if(cost <= 0)
        {
            Debug.LogError("Ticket cost must be greater than zero, YOU FOOL!");
            return false;
        }

        if(!HasCapacity(side, cost))
        {
            return false;
        }

        ticket = new Ticket(ticketID, side, cost);
        activeTickets.Add(ticketID, ticket);

        if(side == AttackSide.Left)
            leftUsedPoints += cost;
        else
            rightUsedPoints += cost;

        return true;
    }

    public void ReleaseTicket(Guid ticketId)
    {
        if (!activeTickets.TryGetValue(ticketId, out var t))
            return;

        activeTickets.Remove(ticketId);

        if(t.Side == AttackSide.Left)
            leftUsedPoints -= t.Cost;
        else
            rightUsedPoints -= t.Cost;

        leftUsedPoints = Mathf.Max(0, leftUsedPoints);
        rightUsedPoints = Mathf.Max(0, rightUsedPoints);
    }

    public bool HasTicket(Guid ticketID) => activeTickets.ContainsKey(ticketID);

    public bool HasCapacity(AttackSide side, int cost)
    {
        if(side == AttackSide.Left)
        {
            return (leftUsedPoints + cost) <= leftSideCapacityPoints;
        }
        else
        {
            return (rightUsedPoints + cost) <= rightSideCapacityPoints;
        }
    }

    public int GetUsed(AttackSide side) => side == AttackSide.Left ? leftUsedPoints : rightUsedPoints;

    public int GetCapacity(AttackSide side) => side == AttackSide.Left ? leftSideCapacityPoints : rightSideCapacityPoints;

    public readonly struct Ticket
    {
        public Guid TicketID { get; }
        public AttackSide Side { get; }
        public int Cost { get; }
        public Ticket(Guid ticketID, AttackSide side, int cost)
        {
            TicketID = ticketID;
            Side = side;
            Cost = cost;
        }
    }
}
