using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCheckTrap : InteractableTrap
{
    private readonly LayerMask _playerLayer = LayerMask.GetMask("Player");

    protected Vector3 _playerCheckCenter = Vector3.zero;
    protected Vector3 _playerCheckHalfSize = Vector3.zero;
    protected Quaternion _playerCheckRotation = Quaternion.identity;

    protected override void Update()
    {
        base.Update();

        if (PlayerCheck())
        {
            OnTrap();
        }
    }

    protected bool PlayerCheck()
    {
        Collider[] playerCheck = Physics.OverlapBox(_playerCheckCenter, _playerCheckHalfSize, _playerCheckRotation, _playerLayer);

        if (playerCheck.Length > 0)
            return true;

        return false;
    }



}
