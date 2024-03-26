using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchNormalAttack : Pattern
{
    public override void OnStart()
    {
        OwnerNode.brain.AnimatorCompo.SetParameterEnable("isAttack");
    }

    public override void OnStop()
    {
        OwnerNode.brain.AnimatorCompo.OnAnimationEnd();
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
