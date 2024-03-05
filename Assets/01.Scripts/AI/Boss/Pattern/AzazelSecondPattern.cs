using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzazelSecondPattern : Pattern
{
    #region Debug
    [Header("Debug objects")]
    public List<GameObject> grigoris;
    #endregion

    public override void OnStart()
    {

    }

    public override void OnStop()
    {

    }

    public override Node.State OnUpdate()
    {
        Debug.Log("Use second pattern.");
        // Pop random grigori object from pool.
        // Debug
        int grigoriIndex = Random.Range(0, grigoris.Count);

        if (grigoris.Count > 0)
        {
            Grigori grigori = Instantiate(grigoris[grigoriIndex]).GetComponent<Grigori>();

            if (grigori.player == null)
            {
                grigori.player = node.blackboard.target.GetComponent<Player>();
            }
        }

        return Node.State.Success;
    }
}
