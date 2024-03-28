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

            if (_normalAttack.OwnerNode == null)
            {
                _normalAttack.OwnerNode = this;
            }

            _normalAttack.OnStart();
        }

        protected override void OnStop()
        {
            _normalAttack.OnStop();
        }

        protected override State OnUpdate()
        {
            if (Time.time <= brain.NormalAttackTimer + brain.EnemyStatData.GetAttackDelay())
            {
                return State.Running;
            }

            brain.NormalAttackTimer = Time.time;

            return _normalAttack.OnUpdate();
        }
    }
}
