using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonHitableTrap : Trap
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!_isPlayTrap)
        {
            OnTrap();
        }
    }

    protected override void PlayTrap()
    {
        Debug.Log("PlayTrap");
    }
}
