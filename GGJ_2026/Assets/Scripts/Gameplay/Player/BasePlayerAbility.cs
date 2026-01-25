using UnityEngine;

public abstract class BasePlayerAbility : ScriptableObject
{
    [Header("Common")]
    public string abilityName = "Ability";
    public float cooldown = 0.2f;

    private float lastUsedTime = -999f;

    public bool IsOffCooldown => (Time.time - lastUsedTime) >= cooldown;

    public bool TryActivate(PlayerContext ctx)
    {
        if (!IsOffCooldown) return false;
        if (!CanActivate(ctx)) return false;

        lastUsedTime = Time.time;
        Activate(ctx);
        return true;
    }

    protected virtual bool CanActivate(PlayerContext ctx) => true;
    protected abstract void Activate(PlayerContext ctx);
}