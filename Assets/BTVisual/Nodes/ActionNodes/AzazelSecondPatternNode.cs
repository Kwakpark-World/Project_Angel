using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class AzazelSecondPatternNode : ActionNode
    {
        public float nextPatternCooldown;

        // Debug
        public List<GameObject> grigoris;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {
            blackboard.selectedPattern = 0;
        }

        protected override State OnUpdate()
        {
            Debug.Log("Use second pattern.");
            // Pop random grigori object from pool.
            // Debug
            int grigoriIndex = Random.Range(0, grigoris.Count);

            if (grigoris.Count > 0)
            {
                Instantiate(grigoris[grigoriIndex]);
            }

            blackboard.nextPatternCooldown = nextPatternCooldown;

            return State.Success;
        }
    }
}
