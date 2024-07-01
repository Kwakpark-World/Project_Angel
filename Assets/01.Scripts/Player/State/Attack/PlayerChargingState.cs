using System.Collections;
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

    private bool _isBlink = false;

    public PlayerChargingState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);

        _player.CurrentChargingTime = 0;
        _isEffectOn = false;
        _isBlink = false;

        _thisParticle = _player.effectParent.Find(_player.IsAwakening ? _awakenEffectString : _effectString).GetComponent<ParticleSystem>();
        var main = _thisParticle.main;
        main.startColor = _player.IsAwakening ? _awakenColor : _normalColor;
        
    }

    public override void Exit()
    {
        base.Exit();
        _thisParticle.Stop();
        _player.PlayerInput.isCharge = false;
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

        CameraManager.Instance.ResetCameraZoom();

        if (_player.CurrentChargingTime < _minChargeTime)
        {
            _player.CurrentChargingTime = 0;
            _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
        }
        else
        {
            _player.chargingPrevTime = Time.time;
            if (_player.IsAwakening)
                _stateMachine.ChangeState(PlayerStateEnum.AwakenChargeAttack);
            else
                _stateMachine.ChangeState(PlayerStateEnum.NormalChargeAttack);
        }
    }

    private void SetChargingGauge()
    {
        if (!_player.PlayerInput.isCharge) return;
        if (_player.chargingPrevTime + _player.PlayerStatData.GetChargingAttackCooldown() > Time.time) return;

        if (_player.CurrentChargingTime < _minChargeTime)
            _player.CurrentChargingTime += Time.deltaTime;
        else
        {            
            ChargingEffect();
            CameraManager.Instance.ZoomCam(5.6f, _cameraZoomChangePerTick);

            _player.CurrentChargingTime += Time.deltaTime * 1.5f;

            if (_player.CurrentChargingTime >= _maxChargeTime - 0.1f)
            {
                if (_isBlink) return;
                _isBlink = true;
                BlinkWeapon();
            }
            
        }


    }

    private void ChargingEffect()
    {
        if (_isEffectOn) return;

        _isEffectOn = true;
        _thisParticle.Play();

        //Vector3 pos = Vector3.zero;
        //EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAttack_Charging_Normal, pos);
    }

    private void SetEffectPos()
    {
        _thisParticle.transform.position = _weaponRT.position;
    }

    private void BlinkWeapon()
    {
        _player.StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        Material weaponMat = _player.materials[(_player.IsAwakening ? (int)PlayerMaterialIndex.Weapon_Awaken : (int)PlayerMaterialIndex.Weapon_Normal)];
        Color color = weaponMat.GetColor("_EmissionColor");

        weaponMat.SetColor("_EmissionColor", color * 10f);
        while (_player.PlayerInput.isCharge)
        {
            yield return null;
        }
        weaponMat.SetColor("_EmissionColor", color);
    }
}
