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
            context.agent.isStopped = false;
            context.agent.destination = blackboard.home;
            _isReturnHome = true;

            if (_detectRange <= 0f)
            {
                _detectRange = brain.EnemyStatData.GetDetectRange();
            }

            brain.AnimatorCompo.SetParameterEnable("isMove");
        }

        protected override void OnStop()
        {
            brain.AnimatorCompo.SetParameterDisable();
            brain.AnimatorCompo.OnAnimationEnd();
        }

        protected override State OnUpdate()
        {
            if ((GameManager.Instance.playerTransform.position - brain.transform.position).sqrMagnitude > _detectRange * _detectRange)
            {
                if (context.agent.remainingDistance <= Mathf.Epsilon)
                {
                    _isReturnHome = !_isReturnHome;

                    if (_isReturnHome)
                    {
                        context.agent.destination = blackboard.home;
                    }
                    else
                    {
                        _patrolDestination = Random.insideUnitCircle.normalized * _patrolRange;
                        context.agent.destination = blackboard.home + new Vector3(_patrolDestination.x, 0f, _patrolDestination.y);
                    }
                }

                return State.Running;
            }

            return State.Success;
        }
    }
}
