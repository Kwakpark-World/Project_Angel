using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzazelFirstPattern : Pattern
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

        if (PoolManager.Instance.Pop(PoolingType.Goat).TryGetComponent(out Goat goat))
        {
            goat.transform.position = node.brain.transform.position;
        }

        return Node.State.Success;
    }
}
