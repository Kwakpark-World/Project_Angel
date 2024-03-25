using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightNormalAttack : Pattern
{
    public override void OnStart()
    {
        OwnerNode.brain.AnimatorCompo.SetParameterEnable("isAttack");
    }

    public override void OnStop()
    {
        OwnerNode.brain.AnimatorCompo.OnAnimationEnd(1);
    }

    public override Node.State OnUpdate()
    {
        if (OwnerNode.brain.AnimatorCompo.GetParameterState("isAttack"))
        {
            return Node.State.Running;
        }

        return Node.State.Success;
    }
}
