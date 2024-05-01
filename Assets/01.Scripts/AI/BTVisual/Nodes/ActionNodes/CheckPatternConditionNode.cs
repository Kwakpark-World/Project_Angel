using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class CheckPatternConditionNode : ActionNode
    {
        public int conditioningPattern;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            return blackboard.selectedPattern == conditioningPattern ? State.Success : State.Failure;
        }
    }
}
