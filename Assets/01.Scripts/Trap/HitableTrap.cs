using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitableTrap : InteractableTrap
{
    protected Vector3 _playerAttackCenter;
    protected Vector3 _playerAttackHalfSize;
    protected Quaternion _playerAttackRotation;

    private bool _isHitTrap;

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        _isHitTrap = false;

        _playerAttackCenter = Vector3.zero;
        _playerAttackHalfSize = Vector3.zero;
        _playerAttackRotation = Quaternion.identity;

        SetPlayerAttackParameter();
    }

    protected override void Update()
    {
        base.Update();

        if (_isHitTrap && !_isPlayTrap)
        {
            OnTrap();
        }
    }

    protected override void PlayTrap(){}

    protected abstract void SetPlayerAttackParameter();
    public void HitTrap()
    {
        _isHitTrap = true;
    }

    public void EndHit()
    {
        _isHitTrap = false;
    }


}
