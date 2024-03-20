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
            _enemySword = node.brain.GetComponentInChildren<EnemySword>();
        }

        _enemySword.swordCollider.enabled = true;

        node.brain.AnimatorCompo.SetBoolEnable("isAttack");

        Debug.Log("Debug");
    }

    public override void OnStop()
    {
        _enemySword.swordCollider.enabled = false;

        node.brain.AnimatorCompo.SetBoolDisable();
    }

    public override Node.State OnUpdate()
    {
        return Node.State.Success;
    }
}
