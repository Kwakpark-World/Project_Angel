using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCheckTrap : InteractableTrap
{
    protected Vector3 _playerCheckCenter;
    protected Vector3 _playerCheckHalfSize;
    protected Quaternion _playerCheckRotation;

    private readonly LayerMask _playerLayer = LayerMask.GetMask("Player");

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        _playerCheckCenter = Vector3.zero;
        _playerCheckHalfSize = Vector3.zero;
        _playerCheckRotation = Quaternion.identity;

        SetPlayerCheckParameter();
    }

    protected override void Update()
    {
        base.Update();

        if (PlayerCheck())
        {
            OnTrap();
        }
    }

    protected override void PlayTrap(){}

    protected abstract void SetPlayerCheckParameter();
    protected bool PlayerCheck()
    {
        Collider[] playerCheck = Physics.OverlapBox(_playerCheckCenter, _playerCheckHalfSize, _playerCheckRotation, _playerLayer);

        if (playerCheck.Length > 0)
            return true;

        return false;
    }



}
