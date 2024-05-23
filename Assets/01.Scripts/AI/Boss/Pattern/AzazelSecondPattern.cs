using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzazelSecondPattern : EnemyAttack
{
    [SerializeField]
    private List<PoolType> grigoris;

    public override void OnStart()
    {

    }

    public override void OnStop()
    {

    }

    public override Node.State OnUpdate()
    {
        Debug.Log("Use second pattern.");

        PoolType grigoriType = grigoris[Random.Range(0, grigoris.Count)];

        if (PoolManager.Instance.Pop(grigoriType, OwnerNode.brain.transform.position).TryGetComponent(out Grigori grigori))
        {
            if (!grigori.owner)
            {
                grigori.owner = OwnerNode.brain as BossBrain;
            }
        }

        return Node.State.Success;
    }
}
