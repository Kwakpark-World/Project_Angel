using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class NormalAttackNode : ActionNode
    {
        public GameObject normalAttackObject;
        private Pattern _normalAttack = null;

        protected override void OnStart()
        {
            context.agent.isStopped = true;

            if (_normalAttack == null)
            {
                _normalAttack = normalAttackObject.GetComponent<Pattern>();
            }

            if (_normalAttack.node == null)
            {
                _normalAttack.node = this;
            }

            _normalAttack.OnStart();
        }

        protected override void OnStop()
        {
            _normalAttack.OnStop();
        }

        protected override State OnUpdate()
        {
            if (Time.time > brain.normalAttackTimer + brain.EnemyStatData.GetAttackDelay())
            {
                brain.normalAttackTimer = Time.time;

                return _normalAttack.OnUpdate();
            }

            return State.Failure;
        }
    }
}
