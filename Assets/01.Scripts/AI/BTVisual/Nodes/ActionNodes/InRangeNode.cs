using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class InRangeNode : ActionNode
    {
        public bool isDetect;
        private float _range;

        protected override void OnStart()
        {
            if (isDetect)
            {
                _range = brain.EnemyStatData.GetDetectRange();
            }
            else
            {
                _range = brain.EnemyStatData.GetAttackRange();
            }
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            return (GameManager.Instance.PlayerInstance.transform.position - brain.transform.position).sqrMagnitude <= _range * _range ? State.Success : State.Failure;
        }
    }
}
