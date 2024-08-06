using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNormalChargeAttackState : PlayerChargeState
{
    private float _width = 11f;
    private float _height = 3f;
    private float _dist = 6f;
    private Vector3 _offset;
    
    private bool _isEffectOn = false;

    private ParticleSystem _thisParticle;

    public PlayerNormalChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _isEffectOn = false;

        // (1 + (max):10%) * speed
        _player.AnimatorCompo.speed = (1 + (_player.CurrentChargeTime / (_maxChargeTime * 10))) * _player.PlayerStatData.GetChargeAttackSpeed();

        if (!_player.isOnChargingSlashOnceMore)
        {
            if (_player.IsAwakened)
            {
                _thisParticle = _player.effectParent.Find(_effectString + "_Awaken").GetComponent<ParticleSystem>();
            }
            else
                _thisParticle = _player.effectParent.Find(_effectString).GetComponent<ParticleSystem>();
        }
        else
        {
            if (_player.IsAwakened)
            {
                _thisParticle = _player.effectParent.Find($"{_effectString}_OnceMore_Awaken").GetComponent<ParticleSystem>();
            }
            else
                _thisParticle = _player.effectParent.Find($"{_effectString}_OnceMore").GetComponent<ParticleSystem>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        _player.enemyNormalHitDuplicateChecker.Clear();
        _thisParticle.Stop();

    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        // 가설. 여기서 이게 계속 돎 그렇기에 때리는 함수에 값을 넘기고 
        if (_isHitAbleTriggerCalled)
            ChargeAttack();

        // Rune 
        if (_actionTriggerCalled)
        {
            if (_player.isChargingSlashOnceMore)
            {
                _player.AnimatorCompo.SetBool("ChargingSlashOnceMore", true);
                _player.isOnChargingSlashOnceMore = true;
                _stateMachine.ChangeState(PlayerStateEnum.NormalChargeAttack);
                return;
            }
        }

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                ChargeAttackEffect();
                if (_player.isChargingSwordAura)
                {
                    Vector3 pos = _player.transform.position + _player.transform.forward;
                    pos.y = 1;

                    EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAttack_Charged_Aura, pos);
                }
            }
        }

        if (_endTriggerCalled)                                                                                
        {
            if (_player.IsPlayerStop == PlayerControlEnum.Stop) return;
            if (_actionTriggerCalled && _player.isChargingSlashOnceMore) return;

            _player.AnimatorCompo.SetBool("ChargingSlashOnceMore", false);
            _player.isOnChargingSlashOnceMore = false;

            _stateMachine.ChangeState(PlayerStateEnum.NormalChargeStabAttack);
        }
    }

    protected override void SetAttackSetting()
    {
        _hitDist = _dist;
        _hitHeight = _height;
        _hitWidth = _width;

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);

        _offset = _player.transform.forward;

        _attackOffset = _offset;
        _attackSize = size;
    }

    private void ChargeAttackEffect()
    {
        _thisParticle.Play();

        //Vector3 pos = _player._weapon.transform.position;
        //EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAttack_Charged_Normal, pos);

    }

    private void ChargeAttack()
    { 
        Collider[] enemies = GetEnemyByOverlapBox(_player.transform.position, _player.transform.rotation);
      
        Attack(enemies.ToList());
    }

    protected override void HitEnemyAction(Brain enemy)
    {
        base.HitEnemyAction(enemy);

        CameraManager.Instance.ShakeCam(0.3f, 0.5f, 0.5f);
    }
}
