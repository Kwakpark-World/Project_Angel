using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : PoolableMonoEffect
{
    protected Player _player;
    public PlayerStateEnum _playerState;

    protected Quaternion _defaultRot;

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();

        _defaultRot = transform.rotation;
    }

    protected override void Update()
    {
        base.Update();

        if (!_player)
        {
            _player = GameManager.Instance.PlayerInstance;
        }
    }
}
