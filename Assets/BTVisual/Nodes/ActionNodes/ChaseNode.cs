using System.Collections;
using System.Collections.Generic;
using BTVisual;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BTVisual
{
    public class ChaseNode : ActionNode
    {
        public float checkDelay = 0.2f; //0.2초마다 체크
        public float chaseRange = 14f;
        public float successRange = 6f;
        private float _timer = 0;

        protected override void OnStart()
        {
            context.agent.destination = blackboard.destination;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (Time.time > _timer + checkDelay)
            {
                if (!InRange())
                {
                    return State.Failure;
                }

                context.agent.destination = blackboard.destination;
                _timer = Time.time;
            }

            if (context.agent.remainingDistance < successRange)
            {
                return State.Success;
            }

            return State.Running;
        }

        private bool InRange()
        {
            Collider[] results = new Collider[1];

            var size = Physics.OverlapSphereNonAlloc(context.transform.position, chaseRange, results, blackboard.enemyLayer);

            if (size >= 1)
            {
                blackboard.destination = results[0].transform.position;

                return true;
            }

            return false;
        }
    }
}
