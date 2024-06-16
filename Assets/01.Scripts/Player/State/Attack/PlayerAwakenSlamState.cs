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
        _player.enemyNormalHitDuplicateChecker.Clear();

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
        if (_comboCounter == 2)
        {
            GameObject obj = GameManager.Instantiate(new GameObject(), _player.transform.position + _attackOffset, _player.transform.rotation);
            obj.transform.localScale = _attackSize;
            obj.AddComponent<MeshFilter>();
            obj.AddComponent<MeshRenderer>();
        }

        Collider[] enemies = GetEnemyByOverlapBox(_player.transform.position, _player.transform.rotation);

        Attack(enemies.ToList());
    }

    protected override void HitEnemyAction(Brain enemy)
    {
        base.HitEnemyAction(enemy);

        CameraManager.Instance.ShakeCam(0.1f, 0.3f, 2f);
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
                _width[_comboCounter] = 6f;
                _height[_comboCounter] = 5f;
                _dist[_comboCounter] = 21f;
                _offset[_comboCounter] = new Vector3(-4.5f, 2.5f, 7);
                break;
            case 2:
                _width[_comboCounter] = 5f;
                _height[_comboCounter] = 6f;
                _dist[_comboCounter] = 13f;
                _offset[_comboCounter] = _player.transform.forward * 2;
                _offset[_comboCounter].y += 2f;
                break;
        }
    }
}
