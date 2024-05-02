using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzazelFirstPattern : EnemyAttack
{
    public override void OnStart()
    {

    }

    public override void OnStop()
    {

    }

    public override Node.State OnUpdate()
    {
        Debug.Log("Use first pattern.");
        PoolManager.Instance.Pop(PoolingType.Enemy_Goat, OwnerNode.brain.transform.position);

        return Node.State.Success;
    }
}
