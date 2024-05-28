using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemistSkillAttack : EnemyAttack
{
    private bool _canSkillPlay;
    private bool _isSkillPlaying;

    public override void OnStart()
    {
        _canSkillPlay = true;
    }

    public override void OnStop()
    {
        OwnerNode.brain.AnimatorCompo.OnAnimationEnd("");
    }

    public override Node.State OnUpdate()
    {
        if (_canSkillPlay)
        {
            _canSkillPlay = false;
            _isSkillPlaying = true;

            OwnerNode.brain.AnimatorCompo.SetAnimationState("SkillAttack");

            return Node.State.Running;
        }
        else if (_isSkillPlaying)
        {
            if (OwnerNode.brain.AnimatorCompo.GetCurrentAnimationState("SkillAttack"))
            {
                return Node.State.Running;
            }

            _isSkillPlaying = false;
        }

        OwnerNode.brain.NormalAttackTimer = OwnerNode.brain.SkillAttackTimer = Time.time;

        return Node.State.Success;
    }
}
