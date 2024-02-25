using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class AzazelNormalAttackNode : ActionNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (Time.time > brain.NormalAttackTimer + brain.normalAttackDelay)
            {
                brain.NormalAttackTimer = Time.time;

                Debug.Log("Attack.");

                return State.Success;
            }

            return State.Failure;
        }
    }
}
