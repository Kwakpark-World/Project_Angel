using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAwakenChargeAttackState : PlayerChargeState
{
    private float _width = 10f;
    private float _height = 3f;
    private float _dist = 10f;
    private Vector3 _offset;

    private const string _animName = "PlayerAttack_Charged_Awaken";

    private bool _isEffectOn = false;
    private bool _isShaken = false;
    private ParticleSystem[] _thisParticles;
    private ParticleSystem _shockwaveParticle;

    private Vector3 defaultScale;


    public PlayerAwakenChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _isEffectOn = false;
        _isShaken = false;

        _player.AnimatorCompo.speed = 1 + (_player.CurrentChargeTime / (_maxChargeTime * 10)) * _player.PlayerStatData.GetChargeAttackSpeed();

        if (_player.isWhirlwindMoveAble)
        {
            _player.PlayerStatData.moveSpeed.AddModifier(-5f);
            _player.AnimatorCompo.speed += 0.2f;
        }

        if (_player.isOnWhirlWindOnceMore)
        {
            _thisParticles = _player.effectParent.Find($"{_effectString}_OnceMore").GetComponentsInChildren<ParticleSystem>();
        }
        else
        {
            _thisParticles = _player.effectParent.Find(_effectString).GetComponentsInChildren<ParticleSystem>();
        }

        defaultScale = _thisParticles[0].transform.localScale;
        if (_player.isWhirlwindRangeUp)
        {
            _thisParticles = _thisParticles.Concat(_player.effectParent.Find($"{_effectString}_Plus").GetComponentsInChildren<ParticleSystem>()).ToArray();

            foreach(var particle in _thisParticles)
            {
                particle.transform.localScale *= 1.2f;
            }
        }

        _shockwaveParticle = _player.effectParent.Find("PlayerShockwave").GetComponent<ParticleSystem>();


    }

    public override void Exit()
    {
        if (_player.isWhirlwindShockWave)
        {
            if (_player.isHit)
            {
                _shockwaveParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _shockwaveParticle.Play();

                _player.enemyNormalHitDuplicateChecker.Clear();

                _attackSize = new Vector3(14f, 2, 14f);
                _attackOffset = new Vector3(0, -0.4f, 0);

                Collider[] enemies = GetEnemyByOverlapBox(_player.transform.position, _player.transform.rotation);

                Attack(enemies.ToList());
            }
        }

        base.Exit();

        _player.AnimatorCompo.speed = 1;
        _player.enemyNormalHitDuplicateChecker.Clear();


        foreach (var particle in _thisParticles)
        {
            particle.Stop();
            
        }
        
        if (_player.isWhirlwindMoveAble)
            _player.PlayerStatData.moveSpeed.RemoveModifier(-5f);

        if (_player.isWhirlwindRangeUp)
        {
            foreach (var particle in _thisParticles)
            {
                particle.transform.localScale = defaultScale;
            }
        }


    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!_player.isWhirlwindMoveAble)
            _player.StopImmediately(false);
        else
        {
            float XInput = _player.PlayerInput.XInput;
            float YInput = _player.PlayerInput.YInput;

            Vector3 moveDir = new Vector3(XInput, 0 , YInput).normalized;

            moveDir = (Quaternion.Euler(0, CameraManager.Instance.GetCameraByType(CameraType.PlayerCam).transform.eulerAngles.y, 0) * moveDir).normalized;
            moveDir *= _player.PlayerStatData.GetMoveSpeed();

            moveDir.y = _rigidbody.velocity.y;
            _player.SetVelocity(moveDir);

            if (_actionTriggerCalled)
            {
                _player.AnimatorCompo.SetBool("WhirlWindOnceMore", true);
                _player.isOnWhirlWindOnceMore = true;

                _player.StateMachine.ChangeState(PlayerStateEnum.AwakenChargeAttack);
                return;
            }
        }

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                ChargeAttackEffect();
            }
        }

        if (_isHitAbleTriggerCalled)
        {
            ChargeAttack();
        }

        if (_endTriggerCalled)
        {
            if (_actionTriggerCalled && _player.isWhirlwindMoveAble) return;
            _player.AnimatorCompo.SetBool("WhirlWindOnceMore", false);
            _player.isOnWhirlWindOnceMore = false;

            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    protected override void SetAttackSetting()
    {
        _hitDist = _dist;
        _hitHeight = _height;
        _hitWidth = _width;

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);

        _offset = Vector3.zero;

        _attackOffset = _offset;
        _attackSize = size;
    }

    private void ChargeAttackEffect()
    {

        float playerDuration = float.MaxValue;
        foreach (var anim in _player.playerAnims)
        {
            if (anim.name == _animName)
            {
                playerDuration = anim.length - 0.84f;
                break;
            }
        }
        if (playerDuration == float.MaxValue)
            Debug.LogError($"Effect : {_effectString} string is not match, this Effect Duration is");

        playerDuration /= _player.AnimatorCompo.speed;

        foreach (var particle in _thisParticles)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = particle.main;
            main.duration = playerDuration;
            particle.Play();
        }

        //Vector3 pos = _player.transform.position;
        //EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAttack_Charged_Awaken, pos);
    }

    private void ChargeAttack()
    {
        if (_TickCheckTriggerCalled) // default One Hit + Tick(Anim Event)
        {
            _TickCheckTriggerCalled = false;
            _isShaken = false;
            _player.enemyNormalHitDuplicateChecker.Clear();
        }

        Collider[] enemies = GetEnemyByOverlapBox(_player.transform.position, _player.transform.rotation);

        if (_player.isWhirlwindPullEnemies)
        {
            foreach (var enemy in enemies)
            {
                Vector3 dir = _player.transform.position - enemy.transform.position;
                enemy.attachedRigidbody.velocity = (dir.normalized) * 2f;
            }
        }

        Attack(enemies.ToList());
    }

    protected override void HitEnemyAction(Brain enemy)
    {
        base.HitEnemyAction(enemy);

        if (_isShaken) return;
        _isShaken = true;
        CameraManager.Instance.ShakeCam(0.1f, 0.3f, 1f);
    }
}
