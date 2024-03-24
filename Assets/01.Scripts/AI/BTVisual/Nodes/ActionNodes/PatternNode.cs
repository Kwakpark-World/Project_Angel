using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class PatternNode : ActionNode
    {
        public GameObject patternObject;
        private Pattern _pattern = null;

        public float nextPatternCooldown;

        protected override void OnStart()
        {
            context.agent.isStopped = true;

            if (_pattern == null)
            {
                _pattern = patternObject.GetComponent<Pattern>();
            }

            if (_pattern.OwnerNode == null)
            {
                _pattern.OwnerNode = this;
            }

            _pattern.OnStart();
        }

        protected override void OnStop()
        {
            blackboard.selectedPattern = 0;

            _pattern.OnStop();
        }

        protected override State OnUpdate()
        {
            blackboard.nextPatternCooldown = nextPatternCooldown;

            return _pattern.OnUpdate();
        }
    }
}
