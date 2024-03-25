using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherNormalAttack : Pattern
{
    public override void OnStart()
    {
        OwnerNode.brain.AnimatorCompo.SetParameterEnable("isReload");
    }

    public override void OnStop()
    {
        OwnerNode.brain.AnimatorCompo.OnAnimationEnd(1);
    }

    public override Node.State OnUpdate()
    {
        if (OwnerNode.brain.AnimatorCompo.GetParameterState("isReload") || OwnerNode.brain.AnimatorCompo.GetParameterState("isAttack"))
        {
            return Node.State.Running;
        }

        return Node.State.Success;
    }
}
