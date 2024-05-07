using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : PoolableMonoEffect
{
    protected Player _player;
    public PlayerStateEnum _playerState;

    protected Quaternion _defaultRot;

    protected override void Awake()
    {
        base.Awake();
        _player = GameManager.Instance.PlayerInstance;
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
