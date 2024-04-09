using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class NormalAttackNode : ActionNode
    {
        public GameObject normalAttackObject;
        private Pattern _normalAttack = null;
        private float _attackRange;

        protected override void OnStart()
        {
            if (_normalAttack == null)
            {
                normalAttackObject = Instantiate(normalAttackObject, brain.transform);
                _normalAttack = normalAttackObject.GetComponent<Pattern>();
            }

            if (_normalAttack.OwnerNode == null)
            {
                _normalAttack.OwnerNode = this;
            }

            if (_attackRange <= 0f)
            {
                _attackRange = brain.EnemyStatData.GetAttackRange();
            }

            _normalAttack.OnStart();
        }

        protected override void OnStop()
        {
            _normalAttack.OnStop();
        }

        protected override State OnUpdate()
        {
            if (!brain.NavMeshAgentCompo.isStopped)
            {
                brain.NavMeshAgentCompo.isStopped = true;
            }

            if ((GameManager.Instance.PlayerInstance.transform.position - brain.transform.position).sqrMagnitude > _attackRange * _attackRange)
            {
                return State.Failure;
            }
            else if (Time.time <= brain.NormalAttackTimer + brain.EnemyStatData.GetAttackDelay())
            {
                return State.Running;
            }

            return _normalAttack.OnUpdate();
        }
    }
}
