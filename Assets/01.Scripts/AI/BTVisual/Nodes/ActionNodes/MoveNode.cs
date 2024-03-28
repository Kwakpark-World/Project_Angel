using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class MoveNode : ActionNode
    {
        protected override void OnStart()
        {
            context.agent.isStopped = false;

            if (context.agent.destination != blackboard.destination)
            {
                context.agent.destination = blackboard.destination;
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
            if (context.agent.remainingDistance > Mathf.Epsilon)
            {
                return State.Running;
            }

            return State.Success;
        }
    }
}
