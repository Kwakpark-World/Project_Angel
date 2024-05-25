using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class AttackNode : ActionNode
    {
        public bool hasSkillAttack;
        public GameObject normalAttackObject;
        public GameObject skillAttackObject;
        private EnemyAttack _normalAttackScript = null;
        private EnemyAttack _skillAttackScript = null;
        private float _attackRange;

        protected override void OnStart()
        {
            if (!_normalAttackScript)
            {
                normalAttackObject = Instantiate(normalAttackObject, brain.transform);

                if (normalAttackObject.TryGetComponent(out _normalAttackScript))
                {
                    _normalAttackScript.OwnerNode = this;
                }
            }

            if (hasSkillAttack && !_skillAttackScript)
            {
                skillAttackObject = Instantiate(skillAttackObject, brain.transform);

                if (skillAttackObject.TryGetComponent(out _skillAttackScript))
                {
                    _skillAttackScript.OwnerNode = this;
                }
            }

            if (_attackRange <= 0f)
            {
                _attackRange = brain.EnemyStatData.GetAttackRange();
            }

            _normalAttackScript?.OnStart();
            _skillAttackScript?.OnStart();
        }

        protected override void OnStop()
        {
            _normalAttackScript?.OnStop();
            _skillAttackScript?.OnStop();
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

            if (hasSkillAttack && (Time.time > brain.SkillAttackTimer + brain.EnemyStatData.GetSkillCooldown()))
            {
                return _skillAttackScript.OnUpdate();
            }
            Debug.Log("¿©±â2");

            return _normalAttackScript.OnUpdate();
        }
    }
}
