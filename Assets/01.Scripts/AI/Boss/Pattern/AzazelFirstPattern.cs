using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzazelFirstPattern : Pattern
{
    #region Debug
    [Header("Debug objects")]
    public GameObject goat;
    #endregion

    public override void OnStart()
    {

    }

    public override void OnStop()
    {

    }

    public override Node.State OnUpdate()
    {
        Debug.Log("Use first pattern.");
        // Pop goat object from pool.
        // Debug
        if (goat)
        {
            Goat goatInstance = Instantiate(goat).GetComponent<Goat>();

            if (goatInstance == null)
            {
                goatInstance.player = node.blackboard.target.GetComponent<Player>();
            }
        }

        return Node.State.Success;
    }
}
