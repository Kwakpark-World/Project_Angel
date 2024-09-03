using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCheckTrap : InteractableTrap
{
    private LayerMask _playerLayer;

    protected Vector3 _playerCheckCenter = Vector3.zero;
    protected Vector3 _playerCheckHalfSize = Vector3.zero;
    protected Quaternion _playerCheckRotation = Quaternion.identity;

    protected bool _isCheckTrap;

    protected override void Awake()
    {
        base.Awake();

        _playerLayer = LayerMask.GetMask("Player");
    }

    protected override void Update()
    {
        base.Update();

        if (PlayerCheck())
        {
            if (_isCheckTrap)
            {
                OnTrap();
            }
            else
            {
                _isCheckTrap = PlayerCheck();
            }
        }
    }

    protected override void EndTrap()
    {
        base.EndTrap();

        _isCheckTrap = false;
    }

    protected bool PlayerCheck()
    {
        Collider[] playerCheck = Physics.OverlapBox(_playerCheckCenter, _playerCheckHalfSize, _playerCheckRotation, _playerLayer);

        if (playerCheck.Length > 0)
            return true;

        return false;
    }



}
