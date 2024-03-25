using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class PatrolNode : ActionNode
    {
        [SerializeField]
        private float _patrolRange;

        private Vector3 _patrolDestination;
        private bool _isReturnHome;
        private float _detectRange;

        protected override void OnStart()
        {
            _patrolDestination = Vector3.zero;
            brain.NavMeshAgentCompo.destination = blackboard.home;
            _isReturnHome = true;

            if (_detectRange <= 0f)
            {
                _detectRange = brain.EnemyStatData.GetDetectRange();
            }
        }

        protected override void OnStop()
        {
            brain.AnimatorCompo.OnAnimationEnd(1);
        }

        protected override State OnUpdate()
        {
            if (brain.NavMeshAgentCompo.isStopped)
            {
                brain.NavMeshAgentCompo.isStopped = false;
            }

            if (!brain.AnimatorCompo.GetParameterState("isMove"))
            {
                brain.AnimatorCompo.SetParameterEnable("isMove");
            }

            if ((GameManager.Instance.playerTransform.position - brain.transform.position).sqrMagnitude > _detectRange * _detectRange)
            {
                if (brain.NavMeshAgentCompo.remainingDistance <= Mathf.Epsilon)
                {
                    _isReturnHome = !_isReturnHome;

                    if (_isReturnHome)
                    {
                        brain.NavMeshAgentCompo.destination = blackboard.home;
                    }
                    else
                    {
                        _patrolDestination = Random.insideUnitCircle.normalized * _patrolRange;
                        brain.NavMeshAgentCompo.destination = blackboard.home + new Vector3(_patrolDestination.x, 0f, _patrolDestination.y);
                    }
                }

                return State.Running;
            }

            return State.Success;
        }
    }
}
