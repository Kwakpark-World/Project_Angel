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
            context.agent.isStopped = false;

            if (_attackRange <= 0f)
            {
                _attackRange = brain.EnemyStatData.GetAttackRange();
            }

            brain.AnimatorCompo.SetParameterEnable("isMove");
        }

        protected override void OnStop()
        {
            brain.AnimatorCompo.SetParameterDisable();
            brain.AnimatorCompo.OnAnimationEnd();
        }

        protected override State OnUpdate()
        {
            if (context.agent.destination != GameManager.Instance.playerTransform.position)
            {
                context.agent.destination = GameManager.Instance.playerTransform.position;
            }

            if (context.agent.remainingDistance > _attackRange)
            {
                return State.Running;
            }

            return State.Success;
        }
    }
}
