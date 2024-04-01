using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class ChaseNode : ActionNode
    {
        private float _attackRange;

        protected override void OnStart()
        {
            if (_attackRange <= 0f)
            {
                _attackRange = brain.EnemyStatData.GetAttackRange();
            }
        }

        protected override void OnStop()
        {
            brain.AnimatorCompo.OnAnimationEnd("");
        }

        protected override State OnUpdate()
        {
            if (brain.NavMeshAgentCompo.isStopped)
            {
                brain.NavMeshAgentCompo.isStopped = false;
            }

            if (brain.AnimatorCompo.GetCurrentAnimationState() != "Move")
            {
                brain.AnimatorCompo.SetAnimationState("Move");
            }

            brain.NavMeshAgentCompo.destination = GameManager.Instance.playerTransform.position;

            if (brain.NavMeshAgentCompo.remainingDistance > _attackRange)
            {
                return State.Running;
            }

            return State.Success;
        }
    }
}
