using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class PatternNode : ActionNode
    {
        public Pattern pattern;
        public float nextPatternCooldown;

        protected override void OnStart()
        {
            if (pattern.node == null)
            {
                pattern.node = this;
            }

            pattern.OnStart();
        }

        protected override void OnStop()
        {
            blackboard.selectedPattern = 0;

            pattern.OnStop();
        }

        protected override State OnUpdate()
        {
            blackboard.nextPatternCooldown = nextPatternCooldown;

            return pattern.OnUpdate();
        }
    }
}
