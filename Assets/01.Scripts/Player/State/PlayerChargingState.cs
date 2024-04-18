using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChargingState : PlayerChargeState
{
    private bool _isChargeParticleOn;

    public PlayerChargingState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _player.RotateToMousePos();

        _player.ChargingGauge = 0;
        _isChargeParticleOn = false;
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.isCharge = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        SetChargingGauge();
        ChargeToNextState();
    }

    private void ChargeToNextState()
    {
        if (_player.PlayerInput.isCharge) return;
        
        if (_player.ChargingGauge < _minChargeTime)
        {
            _player.ChargingGauge = 0;
            _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
        }
        else
        {
            if (_player.IsAwakening)
                _stateMachine.ChangeState(PlayerStateEnum.AwakenChargeAttack);
            else
                _stateMachine.ChangeState(PlayerStateEnum.NormalChargeAttack);
        }
    }

    private void SetChargingGauge()
    {
        if (!_player.PlayerInput.isCharge) return;

        _player.ChargingGauge = Mathf.Clamp(_player.ChargingGauge, 0f, _maxChargeTime);

        if (_player.ChargingGauge < _minChargeTime)
            _player.ChargingGauge += Time.deltaTime;
        else
            _player.ChargingGauge += Time.deltaTime * 1.5f;


    }
}
