using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemistSkillAttack : EnemyAttack
{
    public override void OnStart()
    {
        OwnerNode.brain.AnimatorCompo.SetAnimationState("SkillAttack");
    }

    public override void OnStop()
    {
        OwnerNode.brain.AnimatorCompo.OnAnimationEnd("");
    }

    public override Node.State OnUpdate()
    {
        if (OwnerNode.brain.AnimatorCompo.GetCurrentAnimationState("SkillAttack"))
        {
            return Node.State.Running;
        }

        OwnerNode.brain.NormalAttackTimer = OwnerNode.brain.SkillAttackTimer = Time.time;

        return Node.State.Success;
    }
}
