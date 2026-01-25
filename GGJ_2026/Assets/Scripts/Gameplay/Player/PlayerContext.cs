using UnityEngine;

public enum AbilitySlot
{
    Light,
    Heavy,
    Special
}

public sealed class PlayerContext
{
    public GameObject Owner { get; }
    public Transform Transform { get; }
    public Rigidbody2D Rigidbody { get; }
    public Animator Animator { get; }
    public PlayerMovementController Motor { get; }

    public PlayerContext(GameObject owner)
    {
        Owner = owner;
        Transform = owner.transform;
        Rigidbody = owner.GetComponent<Rigidbody2D>();
        Animator = owner.GetComponentInChildren<Animator>();
        Motor = owner.GetComponent<PlayerMovementController>();
    }
}