using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzazelSecondPattern : Pattern
{
    [SerializeField]
    private List<PoolingType> grigoris;

    public override void OnStart()
    {

    }

    public override void OnStop()
    {

    }

    public override Node.State OnUpdate()
    {
        Debug.Log("Use second pattern.");

        PoolingType grigoriType = grigoris[Random.Range(0, grigoris.Count)];

        if (GameManager.Instance.pool.Pop(grigoriType).TryGetComponent(out Grigori grigori))
        {
            if (grigori.player)
            {
                grigori.player = GameManager.Instance.player;
                grigori.owner = node.brain as BossBrain;
            }
        }

        return Node.State.Success;
    }
}
