using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class MoveNode : ActionNode
    {
        protected override void OnStart()
        {
            context.agent.isStopped = true;

            if (context.agent.destination != blackboard.destination)
            {
                context.agent.destination = blackboard.destination;
            }
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (context.agent.remainingDistance > Mathf.Epsilon)
            {
                return State.Running;
            }

            return State.Success;
        }
    }
}
