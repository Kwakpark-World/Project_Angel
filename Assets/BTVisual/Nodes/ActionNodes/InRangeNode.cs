using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class InRangeNode : ActionNode
    {
        public bool isDetect;
        private Transform _target;
        private float _range;

        protected override void OnStart()
        {
            if (!_target)
            {
                _target = GameManager.Instance.player.transform;
            }

            if (isDetect)
            {
                _range = brain.EnemyStatistic.GetDetectRange();
            }
            else
            {
                _range = brain.EnemyStatistic.GetAttackRange();
            }
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            return (brain.transform.position - _target.position).sqrMagnitude <= _range * _range ? State.Success : State.Failure;
        }
    }
}
