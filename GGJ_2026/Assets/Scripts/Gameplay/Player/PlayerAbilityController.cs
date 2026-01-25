using System;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [Serializable]
    public struct AbilitySlotBinding
    {
        public AbilitySlot slot;
        public BasePlayerAbility ability;
    }

    [Header("Slots")]
    [SerializeField] private AbilitySlotBinding[] bindings;

    private PlayerContext ctx;

    private void Awake()
    {
        ctx = new PlayerContext(gameObject);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
            TryUse(AbilitySlot.Light);
        if (Input.GetButtonDown("Fire2")) 
            TryUse(AbilitySlot.Heavy);
        if (Input.GetButtonDown("Fire3")) 
            TryUse(AbilitySlot.Special);
    }

    public bool TryUse(AbilitySlot slot)
    {
        var ability = GetAbility(slot);

        if (ability == null) 
            return false;

        return ability.TryActivate(ctx);
    }

    public BasePlayerAbility GetAbility(AbilitySlot slot)
    {
        for (int i = 0; i < bindings.Length; i++)
            if (bindings[i].slot == slot)
                return bindings[i].ability;

        return null;
    }

    public void SetAbility(AbilitySlot slot, BasePlayerAbility ability)
    {
        for (int i = 0; i < bindings.Length; i++)
        {
            if (bindings[i].slot == slot)
            {
                bindings[i].ability = ability;
                return;
            }
        }
    }
}
