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

        Vector3 pos = _player.transform.position;
        if (_player.IsAwakening)
        {
            pos += _player.transform.forward;
            pos.y += 2f;
            //EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charged_Awaken, pos);
        }
        else
        {
            pos += _player.transform.right * 2;
            //EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charged_Normal, pos);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.isCharge = false;
        _isChargeParticleOn = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _player.ChargingGauge = Mathf.Clamp(_player.ChargingGauge, 0f, _maxChargeTime);

        if (!_player.PlayerInput.isCharge)
        {
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
        else
        {
            if (_player.ChargingGauge < _minChargeTime)
                _player.ChargingGauge += Time.deltaTime;
            else
                _player.ChargingGauge += Time.deltaTime * 1.5f;
        }


        if (_effectTriggerCalled)
        {
            if (!_isChargeParticleOn)
            {
                _isChargeParticleOn = true;

                if (_player.IsAwakening)
                {
                    EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charging_Awaken, _weaponRT.position);
                }
                else
                {
                    EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charging_Normal, _weaponRT.position);
                }
            }
        }

    }
}
