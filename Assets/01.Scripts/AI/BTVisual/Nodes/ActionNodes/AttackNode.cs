using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class AttackNode : ActionNode
    {
        public bool isSkillAttack;
        public GameObject attackObject;
        private EnemyAttack _attackScript = null;
        private float _attackRange;

        protected override void OnStart()
        {
            if (_attackScript == null)
            {
                attackObject = Instantiate(attackObject, brain.transform);
                _attackScript = attackObject.GetComponent<EnemyAttack>();
            }

            if (_attackScript.OwnerNode == null)
            {
                _attackScript.OwnerNode = this;
            }

            if (_attackRange <= 0f)
            {
                _attackRange = brain.EnemyStatData.GetAttackRange();
            }

            _attackScript.OnStart();
        }

        protected override void OnStop()
        {
            _attackScript.OnStop();
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
            else if (isSkillAttack && Time.time <= brain.SkillAttackTimer + brain.EnemyStatData.GetSkillCooldown())
            {
                return State.Failure;
            }
            else if (!isSkillAttack && Time.time <= brain.NormalAttackTimer + brain.EnemyStatData.GetAttackDelay())
            {
                return State.Running;
            }

            return _attackScript.OnUpdate();
        }
    }
}
