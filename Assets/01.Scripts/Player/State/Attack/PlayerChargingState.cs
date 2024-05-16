using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChargingState : PlayerChargeState
{
    private ParticleSystem _thisParticle;

    private Color _normalColor = new Color(1, 0.9592881f, 0.4858491f, 1f);
    private Color _awakenColor = Color.red;

    public PlayerChargingState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _player.RotateToMousePos();

        _player.ChargingGauge = 0;

        _thisParticle = _player.effectParent.Find(_effectString).GetComponent<ParticleSystem>();
        var main = _thisParticle.main;
        main.startColor = _player.IsAwakening ? _awakenColor : _normalColor;
        
        ChargingEffect();

        
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.isCharge = false;
        _thisParticle.Stop();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        SetEffectPos();
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

    private void ChargingEffect()
    {
        _thisParticle.Play();

        //Vector3 pos = Vector3.zero;
        //EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charging_Normal, pos);
    }

    private void SetEffectPos()
    {
        _thisParticle.transform.position = _weaponRT.position;
    }
}
