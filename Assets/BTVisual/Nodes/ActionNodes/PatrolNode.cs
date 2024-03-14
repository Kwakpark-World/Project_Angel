using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class PatrolNode : ActionNode
    {
        private Vector3 _patrolDestination;
        private bool _isReturnHome;
        private float _detectRange;

        protected override void OnStart()
        {
            _patrolDestination = Vector3.zero;
            context.agent.isStopped = false;
            _isReturnHome = true;

            if (_detectRange <= 0f)
            {
                _detectRange = brain.EnemyStatistic.GetDetectRange();
            }
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (_isReturnHome && context.agent.destination != blackboard.home)
            {
                context.agent.destination = blackboard.home;

                Debug.Log("집");
            }
            else if (!_isReturnHome && context.agent.destination != _patrolDestination)
            {
                _patrolDestination = Random.insideUnitCircle;

                _patrolDestination.Normalize();

                _patrolDestination = brain.transform.position + new Vector3(_patrolDestination.x, 0f, _patrolDestination.y);
                context.agent.destination = _patrolDestination;

                Debug.Log("순찰");
            }

            if ((GameManager.Instance.playerTransform.position - brain.transform.position).sqrMagnitude > _detectRange * _detectRange)
            {
                if (context.agent.remainingDistance <= Mathf.Epsilon)
                {
                    _isReturnHome = !_isReturnHome;

                    Debug.Log("상태 반전");
                }

                return State.Running;
            }

            return State.Success;
        }
    }
}
