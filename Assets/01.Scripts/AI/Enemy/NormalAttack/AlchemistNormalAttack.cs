using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistNormalAttack : EnemyAttack
{
    public override void OnStart()
    {
        OwnerNode.brain.AnimatorCompo.SetAnimationState("NormalAttack");
    }

    public override void OnStop()
    {
        OwnerNode.brain.AnimatorCompo.OnAnimationEnd("");
    }

    public override Node.State OnUpdate()
    {
        if (OwnerNode.brain.AnimatorCompo.GetCurrentAnimationState() == "NormalAttack")
        {
            return Node.State.Running;
        }

        OwnerNode.brain.NormalAttackTimer = Time.time;

        return Node.State.Success;
    }
}
