using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Melee Bonk")]
public class MeleeBonkAbility : BasePlayerAbility
{
    public float windupTime = 0.1f;
    public float activeTime = 0.12f;
    public float moveLockTime = 0.22f;

    protected override void Activate(PlayerContext ctx)
    {
        if (ctx.Motor != null)
            ctx.Owner.GetComponent<MonoBehaviour>().StartCoroutine(LockMovement(ctx));

        //just put something random here for now to represent attack, this isn't functional or anything rn lol
        if (ctx.Animator != null)
            ctx.Animator.SetTrigger("Attack");
    }

    private System.Collections.IEnumerator LockMovement(PlayerContext ctx)
    {
        //found this on a google search, not convinced it works.
        ctx.Motor.SetMovementEnabled(false);

        yield return new WaitForSeconds(moveLockTime);

        ctx.Motor.SetMovementEnabled(true);
    }
}
