using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitableTrap : InteractableTrap
{
    private bool _isHitTrap;


    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        _isHitTrap = false;
    }

    protected override void Update()
    {
        base.Update();

        if (_isHitTrap)
        {
            OnTrap();
        }
    }

    public void HitTrap()
    {
        _isHitTrap = true;
    }

    public void EndHit()
    {
        _isHitTrap = false;
    }


}
