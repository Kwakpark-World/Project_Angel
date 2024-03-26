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
            if (brain.AnimatorCompo.GetParameterState("isDie"))
            {
                return;
            }

            if (_normalAttack == null)
            {
                normalAttackObject = Instantiate(normalAttackObject, brain.transform);
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
            if (brain.AnimatorCompo.GetParameterState("isDie"))
            {
                return;
            }

            _normalAttack.OnStop();
        }

        protected override State OnUpdate()
        {
            if (brain.AnimatorCompo.GetParameterState("isDie"))
            {
                return State.Failure;
            }

            if (!brain.NavMeshAgentCompo.isStopped)
            {
                brain.NavMeshAgentCompo.isStopped = true;
            }

            if (Time.time <= brain.NormalAttackTimer + brain.EnemyStatData.GetAttackDelay())
            {
                return State.Running;
            }

            brain.NormalAttackTimer = Time.time;

            return _normalAttack.OnUpdate();
        }
    }
}
