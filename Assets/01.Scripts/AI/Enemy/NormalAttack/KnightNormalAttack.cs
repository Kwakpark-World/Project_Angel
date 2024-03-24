using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightNormalAttack : Pattern
{
    private EnemySword _enemySword;

    public override void OnStart()
    {
        if (_enemySword == null)
        {
            _enemySword = OwnerNode.brain.GetComponentInChildren<EnemySword>();
        }

        OwnerNode.brain.AnimatorCompo.SetParameterEnable("isAttack");
    }

    public override void OnStop()
    {
        OwnerNode.brain.AnimatorCompo.SetParameterDisable();
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
