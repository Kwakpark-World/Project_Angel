using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private Vector3[] _offset = new Vector3[3];

    private PoolableMono particle;

    public PlayerAwakenSlamState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

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

                particle = EffectManager.Instance.PlayAndGetEffect((PoolType)Enum.Parse(typeof(PoolType), _comboEffectString), _effectPos);
                
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
        InitSettings();

        _hitDist = _dist[_comboCounter];
        _hitHeight = _height[_comboCounter];
        _hitWidth = _width[_comboCounter];

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);

        _attackOffset = _offset[_comboCounter];
        _attackSize = size;
    }

    private void SlamAttack()
    {
        Vector3 startPos, endPos;
        InitAttackPosSetting(out startPos, out endPos);

        float startY = startPos.y;

        startPos.y = 0;
        endPos.y = 0;

        Vector3 dir = (endPos - startPos).normalized;
        startPos.y = startY;

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

    private void InitSettings()
    {
        switch (_comboCounter)
        {
            case 0:
                _width[_comboCounter] = 7f;
                _height[_comboCounter] = 7f;
                _dist[_comboCounter] = 20f;
                _offset[_comboCounter] = new Vector3(1.5f, 3f, 7f);
                break;
            case 1:
                _width[_comboCounter] = 5f;
                _height[_comboCounter] = 5f;
                _dist[_comboCounter] = 20f;
                _offset[_comboCounter] = new Vector3(-2f, 2.5f, 8);
                break;
            case 2:
                _width[_comboCounter] = 5f;
                _height[_comboCounter] = 6f;
                _dist[_comboCounter] = 15f;
                _offset[_comboCounter] = _player.transform.forward * 5;
                _offset[_comboCounter].y += 2f;
                break;
        }
    }

    private void InitAttackPosSetting(out Vector3 startPos, out Vector3 endPos)
    {
        if (_comboCounter < 2)
        {
            startPos = particle.transform.Find("t1").position;
            endPos = particle.transform.Find("t3").position;
        }
        else
        {
            startPos = _player.transform.forward * 3;
            endPos = _player.transform.forward * 4;
        }
    }
}
