using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChargeState : PlayerState
{
    private float _minChargeTime = 0.5f;
    private float _maxChargeTime = 2f;

    private bool _isChargeParticleOn;

    public PlayerChargeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _player.RotateToMousePos();

        _player.ChargingGage = 0;
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

        _player.ChargingGage = Mathf.Clamp(_player.ChargingGage, 0f, _maxChargeTime);

        if (!_player.PlayerInput.isCharge)
        {
            if (_player.ChargingGage < _minChargeTime)
            {
                _player.ChargingGage = 0;
                _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
            }
            else
            {
                if (_player.IsAwakening)
                    _stateMachine.ChangeState(PlayerStateEnum.EChargeAttack);
                else
                    _stateMachine.ChangeState(PlayerStateEnum.ChargeAttack);
            }
        }
        else
        {
            if (_player.ChargingGage < _minChargeTime)
                _player.ChargingGage += Time.deltaTime;
            else
                _player.ChargingGage += Time.deltaTime * 1.5f;
        }
        

        if (_effectTriggerCalled)
        {
            if (!_isChargeParticleOn)
            {
                _isChargeParticleOn = true;

                if (_player.IsAwakening)
                {
                    EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charging_Awaken, _player._weapon.transform.Find("RightPointTop").position);
                }
                else
                {
                    EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charging_Normal, _player._weapon.transform.Find("RightPointTop").position);
                }
            }
        }
        
    }
}
