using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class NormalAttackNode : ActionNode
    {
        private Player player = null;

        protected override void OnStart()
        {
            if (player == null)
            {
                player = blackboard.target.GetComponent<Player>();
            }
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (Time.time > brain.NormalAttackTimer + brain.normalAttackDelay)
            {
                brain.NormalAttackTimer = Time.time;

                player.TakeDamage(brain.attackPower);

                return State.Success;
            }

            return State.Failure;
        }
    }
}
