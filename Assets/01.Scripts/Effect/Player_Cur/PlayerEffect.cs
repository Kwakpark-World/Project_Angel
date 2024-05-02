using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : PoolableMonoEffect
{
    protected Player _player;
    public PlayerStateEnum _playerState;

    private PlayerAttackState _playerAttackState;
    protected float _hitDist;

    protected Quaternion _defaultRot;

    protected override void Awake()
    {
        base.Awake();
        _player = GameManager.Instance.PlayerInstance;

        _playerAttackState = _player.StateMachine.GetState(_playerState) as PlayerAttackState;
        if (_playerAttackState != null)
        {
            _hitDist = _playerAttackState._hitDist;
        }
    }

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();

        _defaultRot = transform.rotation;
    }

    protected override void Update()
    {
        base.Update();
    }
}
