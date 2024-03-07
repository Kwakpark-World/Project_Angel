using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class NormalAttackNode : ActionNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (Time.time > brain.NormalAttackTimer + brain.NormalAttackTimer)
            {
                brain.NormalAttackTimer = Time.time;

                GameManager.Instance.player.PlayerStat.Hit(brain.EnemyStatistic.GetAttackPower());

                return State.Success;
            }

            return State.Failure;
        }
    }
}
