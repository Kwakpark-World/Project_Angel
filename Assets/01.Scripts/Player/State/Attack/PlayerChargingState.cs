using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChargingState : PlayerChargeState
{
    private ParticleSystem _thisParticle;

    private Color _normalColor = new Color(1, 0.9592881f, 0.4858491f, 1f);
    private Color _awakenColor = Color.red;

    private const string _awakenEffectString = "PlayerAwakenChargingEffect";

    private bool _isEffectOn;

    private float _cameraZoomChangePerTick = 0.1f;

    public PlayerChargingState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _player.RotateToMousePos();

        _player.ChargingGauge = 0;
        _isEffectOn = false;

        _thisParticle = _player.effectParent.Find(_player.IsAwakening ? _awakenEffectString : _effectString).GetComponent<ParticleSystem>();
        var main = _thisParticle.main;
        main.startColor = _player.IsAwakening ? _awakenColor : _normalColor;
        
    }

    public override void Exit()
    {
        base.Exit();
        _thisParticle.Stop();
        _player.PlayerInput.isCharge = false;

        CameraManager.Instance.ResetCameraZoom();

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
        {
            ChargingEffect();
            CameraManager.Instance.ZoomCam(5f, _cameraZoomChangePerTick);

            _player.ChargingGauge += Time.deltaTime * 1.5f;
        }


    }

    private void ChargingEffect()
    {
        if (_isEffectOn) return;

        _isEffectOn = true;
        _thisParticle.Play();

        //Vector3 pos = Vector3.zero;
        //EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charging_Normal, pos);
    }

    private void SetEffectPos()
    {
        _thisParticle.transform.position = _weaponRT.position;
    }
}
