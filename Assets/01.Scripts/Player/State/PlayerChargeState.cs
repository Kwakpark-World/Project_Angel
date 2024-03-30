using UnityEngine;

public class PlayerChargeState : PlayerState
{
    private float _minChargeTime = 0.5f;
    private float _maxChargeTime = 2f;



    public PlayerChargeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _player.RotateToMousePos();

        _player.ChargingGage = 0;

        EffectManager.Instance.PlayEffect(PoolingType.PlayerChargeEffect, _player._currentWeapon.transform.Find("Point").position);

        Vector3 pos = _player.transform.position;
        pos += _player.transform.right * 2;
        pos.y += 2;

        EffectManager.Instance.PlayEffect(PoolingType.PlayerChargeAttackEffect, pos);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _player.ChargingGage = Mathf.Clamp(_player.ChargingGage, 0f, _maxChargeTime);

        if (!_player.PlayerInput.isCharge)
        {
            if (_player.ChargingGage < _minChargeTime)
            {
                _player.ChargingGage = 0;
                _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
            }
            else
                _stateMachine.ChangeState(PlayerStateEnum.ChargeAttack);
        }
        else
        {
            if (_player.ChargingGage < _minChargeTime)
                _player.ChargingGage += Time.deltaTime;
            else
                _player.ChargingGage += Time.deltaTime * 1.5f;
        }
        
        
    }
}
