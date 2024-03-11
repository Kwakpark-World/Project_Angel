using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class InRangeNode : ActionNode
    {
        public float range;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            Collider[] results = new Collider[1];

            var size = Physics.OverlapSphereNonAlloc(context.transform.position, range, results, blackboard.enemyLayer);

            if (size >= 1)
            {
                blackboard.destination = results[0].transform.position;

                return State.Success;
            }

            return State.Failure;
        }
    }
}
