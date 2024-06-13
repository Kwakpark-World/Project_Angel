using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAwakenSlamState : PlayerAttackState
{
    private readonly int _comboCounterHash = Animator.StringToHash("SlamComboCounter");
    private int _comboCounter;
    private string _comboEffectString;

    private float _attackPrevTime;
    private float _comboWindow = 2.0f;

    private bool _isCombo = false;
    private bool _isEffectOn = false;

    private Vector3 _effectPos = Vector3.zero;

    private float[] _width = new float[3];
    private float[] _height = new float[3];
    private float[] _dist = new float[3];

    public PlayerAwakenSlamState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        InitSetting();

        _player.PlayerInput.SlamSkillEvent += ComboSkill;
        _player.RotateToMousePos();

        _isEffectOn = false;

        _player.SetVelocity(_player.transform.forward * (_comboCounter + 1) * 5f);

        SetCombo();

        _comboEffectString = $"Effect_PlayerAttack_Slam_Awaken_{_comboCounter}";

        _player.StartDelayAction(0.1f, () =>
        {
            _player.StopImmediately(false);
        });
    }

    public override void Exit()
    {
        base.Exit();

        _attackPrevTime = Time.time;
        ++_comboCounter;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;

                if (_comboCounter != 1)
                {
                    _effectPos = _player.transform.position;
                }
                switch (_comboCounter)
                {
                    case 0: _effectPos.x += 2f; break;
                    case 1: _effectPos.x += -3.4f; break;
                }

                EffectManager.Instance.PlayEffect((PoolType)Enum.Parse(typeof(PoolType), _comboEffectString), _effectPos);
            }
        }

        if (_isHitAbleTriggerCalled)
        {
            SlamAttack();
        }

        if (_endTriggerCalled)
        {
            if (_isCombo)
                _stateMachine.ChangeState(PlayerStateEnum.AwakenSlam);
            else
                _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    protected override void SetAttackSetting()
    {
        _hitDist = _dist[_comboCounter];
        _hitHeight = _height[_comboCounter];
        _hitWidth = _width[_comboCounter];

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);

        _attackOffset = _player.transform.forward * 3f;
        _attackSize = size;
    }

    private void SlamAttack()
    {
        Vector3 startPos = Vector3.zero;
        Vector3 dir = Vector3.zero;

        Collider[] enemies = GetEnemyByOverlapBox(startPos, Quaternion.LookRotation(dir));

        Attack(enemies.ToList());
    }

    private void ComboSkill()
    {
        if (_comboCounter >= 2) return;
        _isCombo = true;
    }

    private void SetCombo()
    {
        _isCombo = false;

        if (_comboCounter >= 3 || Time.time >= _attackPrevTime + _comboWindow)
            _comboCounter = 0;

        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);
    }

    private void InitSetting()
    {
        _width[0] = 0f;
        _width[1] = 0f;
        _width[2] = 0f;

        _height[0] = 0f;
        _height[1] = 0f;
        _height[2] = 0f;

        _dist[0] = 0f;
        _dist[1] = 0f;
        _dist[2] = 0f;
    }
}
