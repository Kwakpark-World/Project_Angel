using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class AzazelFirstPatternNode : ActionNode
    {
        public float nextPatternCooldown;
        // Debug
        public GameObject goat;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {
            blackboard.selectedPattern = 0;
        }

        protected override State OnUpdate()
        {
            Debug.Log("Use first pattern.");
            // Pop goat object from pool.
            // Debug
            if (goat)
            {
                Instantiate(goat);
            }

            blackboard.nextPatternCooldown = nextPatternCooldown;

            return State.Success;
        }
    }
}
