using System.Collections;
using System.Collections.Generic;
using BTVisual;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

namespace BTVisual
{
    public class ChaseNode : ActionNode
    {
        private Transform _target;
        private float _attackRange;

        protected override void OnStart()
        {
            if (!_target)
            {
                _target = GameManager.Instance.player.transform;
            }

            if (_attackRange <= 0f)
            {
                _attackRange = brain.EnemyStatistic.GetAttackRange();
            }
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (context.agent.destination != _target.position)
            {
                context.agent.destination = blackboard.destination = _target.position;
            }

            if ((brain.transform.position - _target.position).sqrMagnitude <= _attackRange * _attackRange)
            {
                bool debug = true;

                if (debug/* Use raycast here.*/)
                {
                    return State.Success;
                }
            }

            return State.Running;
        }
    }
}
