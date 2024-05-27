using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonHitableTrap : Trap
{

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();
    }

    protected override void Update()
    {
        base.Update();
        
        OnTrap();
        
    }

    protected override void PlayTrap(){}
}
