using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkillAttack : EnemyAttack
{
    public override void OnStart()
    {

    }

    public override void OnStop()
    {

    }

    public override Node.State OnUpdate()
    {
        return Node.State.Success;
    }
}
