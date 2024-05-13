using BTVisual;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkillAttack : EnemyAttack
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
            StartCoroutine(ArcherSkillCoroutine());

            return Node.State.Running;
        }
        else if (_isSkillPlaying)
        {
            return Node.State.Running;
        }

        OwnerNode.brain.NormalAttackTimer = OwnerNode.brain.SkillAttackTimer = Time.time;

        return Node.State.Success;
    }

    private IEnumerator ArcherSkillCoroutine()
    {
        _canSkillPlay = false;
        _isSkillPlaying = true;

        for (int i = 0; i < 5; ++i)
        {
            OwnerNode.brain.AnimatorCompo.SetAnimationState("SkillReload");

            yield return new WaitUntil(() => OwnerNode.brain.AnimatorCompo.GetCurrentAnimationState("Idle"));
        }

        _isSkillPlaying = false;
    }
}
