using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class MoveNode : ActionNode
    {
        protected override void OnStart()
        {
            brain.NavMeshAgentCompo.destination = blackboard.destination;
        }

        protected override void OnStop()
        {
            brain.AnimatorCompo.OnAnimationEnd(1);
        }

        protected override State OnUpdate()
        {
            if (brain.AnimatorCompo.GetParameterState("isDie"))
            {
                return State.Failure;
            }

            if (brain.NavMeshAgentCompo.isStopped)
            {
                brain.NavMeshAgentCompo.isStopped = false;
            }

            if (!brain.AnimatorCompo.GetParameterState("isMove"))
            {
                brain.AnimatorCompo.SetParameterEnable("isMove");
            }

            if (brain.NavMeshAgentCompo.remainingDistance > Mathf.Epsilon)
            {
                return State.Running;
            }

            return State.Success;
        }
    }
}
