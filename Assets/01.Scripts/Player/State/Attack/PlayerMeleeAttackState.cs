using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttackState : PlayerAttackState
{
    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    public int _comboCounter;

    private float _attackPrevTime;
    private float _comboWindow = 0.8f;
    private float _comboAttackAddtiveDamage = 1f;

    private float _width = 0.8f;
    private float _height = 0.4f;
    private float _dist = 2.6f;
    private Vector3 _offset;

    private float _awakenAttackDist = 2.6f;
    private float _normalAttackDist = 2.6f;

    private bool _isCombo;
    private bool _isEffectOn;

    private ParticleSystem _thisParticle; 

    public PlayerMeleeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MeleeAttackEvent += ComboAttack;
        _player.PlayerStatData.attackPower.InitializeModifier();
        _player.RotateToMousePos();

        _player.IsAttack = true;
        _isEffectOn = false;

        _player.AnimatorCompo.speed = _player.PlayerStatData.GetAttackSpeed();

        SetCombo();
        _player.PlayerStatData.attackPower.AddModifier(_comboAttackAddtiveDamage * _comboCounter);

        _hitDist = _player.IsAwakening ? _awakenAttackDist : _normalAttackDist;

        _player.StartDelayAction(0.1f, () =>
        {
            _player.StopImmediately(false);
        });

        _thisParticle = _player.weapon.transform.Find(_effectString).GetComponent<ParticleSystem>(); 


    }

    public override void Exit()
    {
        base.Exit();

        _player.PlayerInput.MeleeAttackEvent -= ComboAttack;
        _player.IsAttack = false;

        _player.AnimatorCompo.speed = 1f;

        _attackPrevTime = Time.time;

        ++_comboCounter;
        _player.enemyNormalHitDuplicateChecker.Clear();
        _thisParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public override void UpdateState()
    {
        base.UpdateState();


        if (_isHitAbleTriggerCalled)
        {
            if (!_actionTriggerCalled)
                MeleeAttack();
        }

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                ChargingEffect();
            }
        }

        if (_effectTriggerEndCalled)
        {
            _thisParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        if (_actionTriggerCalled)
        {
            if (_player.IsGroundDetected())
            {
                if (_isCombo)
                {
                    _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
                    return;
                }
            }
        }

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    protected override void SetAttackSetting()
    {
        _hitDist = _dist;
        _hitHeight = _height;
        _hitWidth = _width;

        Vector3 size = new Vector3(_hitWidth, _hitDist, _hitHeight);

        _offset = _player.weapon.transform.up;

        _attackOffset = _offset;
        _attackSize = size;
    }

    private void MeleeAttack()
    {
        //List<RaycastHit> enemies = GetEnemyByWeapon();
        //
        //Attack(enemies);

        Collider[] enemies = GetEnemyByOverlapBox(_player.weapon.transform.position, _player.weapon.transform.rotation);

        Attack(enemies.ToList());

        GuidedBulletFire(5f);
    }

    IEnumerator GuidedBulletFire(float delay)
    {
        delay += Time.deltaTime;
        if (_player.BuffCompo.GetBuffState(BuffType.Rune_Attack_Michael) && delay < Time.deltaTime)
        {
            PoolManager.Instance.Pop(PoolType.GuidedBullet, GameManager.Instance.PlayerInstance.transform.position);
            delay = 0;
        }

        yield return new WaitForSeconds(delay);
    }

    protected override void HitEnemyAction(Brain enemy)
    {
        base.HitEnemyAction(enemy);

        if (_comboCounter == 3)
        {
            CameraManager.Instance.ShakeCam(0.2f, 0.5f, 0.5f);
            if(_player.BuffCompo.GetBuffState(BuffType.Rune_Attack_Thor))
            {
                //여기에 파티클까지 나와야함 근데 파티클에 데미지 자체를 넣는 방법도 있긴함 그건 알아서 생각을 해보시길..
                _player.PlayerStatData.attackPower.AddModifier(3f);
                Debug.Log("2");
            }
        }
    }

    private void ChargingEffect()
    {
        _thisParticle.Play();
        //Vector3 pos = Vector3.zero;
        //EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAttack_Charging_Normal, pos);
    }

    private void SetCombo()
    {
        _isCombo = false;

        if (_comboCounter >= 4 || Time.time >= _attackPrevTime + _comboWindow)
            _comboCounter = 0;

        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);

    }

    private void ComboAttack()
    {
        _isCombo = true;
    }

    public void UpgradeActivePoison()
    {
        //TODO: use poison every attack
    }

    public void KillInactivePoison()
    {

    }
}
