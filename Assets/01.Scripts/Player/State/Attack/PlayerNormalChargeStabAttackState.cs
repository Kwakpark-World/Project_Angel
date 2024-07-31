using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNormalChargeStabAttackState : PlayerChargeState
{
    private float _width = 4f;
    private float _height = 8f;
    private float _dist = 12f;

    private bool _isStabMove;
    private bool _isEffectOn = false;

    private ParticleSystem[] _thisParticle = new ParticleSystem[3];


    public PlayerNormalChargeStabAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _isEffectOn = false;
        _isStabMove = false;

        for (int i = 0; i < 3; i++)
        {
            _thisParticle[i] = _player.effectParent.Find(_effectString + i).GetComponent<ParticleSystem>();
        }

        CameraManager.Instance._currentCam.IsCamRotateStop = true;
    }

    public override void Exit()
    {
        base.Exit();
        CameraManager.Instance._currentCam.IsCamRotateStop = false;

        _player.enemyNormalHitDuplicateChecker.Clear();

        _player.CurrentChargeTime = 0;
        _player.AnimatorCompo.speed = 1;

        for (int i = 0; i < 3; i++)
        {
            _thisParticle[i].Stop();
        }

    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_isHitAbleTriggerCalled)
        {
            ChargeAttackStab();
        }

        if (_TickCheckTriggerCalled)
        {
            if (_player.isChargingMultipleSting)
            {
                _TickCheckTriggerCalled = false;
                _isEffectOn = false;
                _player.enemyNormalHitDuplicateChecker.Clear();
            }

        }

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                ChargeAttackStabEffect();
            }
        }        

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (_actionTriggerCalled)
        {
            MoveToFront();
        }
    }

    protected override void SetAttackSetting()
    {
        _hitDist = _dist;
        _hitHeight = _height;
        _hitWidth = _width;

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);

        _attackOffset = Vector3.zero;
        _attackSize = size;
    }

    private void ChargeAttackStabEffect()
    {
        
        Vector3 pos = _weaponRB.transform.position;
        pos.x = 0;

        for (int i = 0; i < 3; i++)
        {
            _thisParticle[i].transform.position = pos;
            _thisParticle[i].Play();
            if (!_player.isChargingTripleSting)
                break;
        }


        //Vector3 pos = _player._weapon.transform.position;
        //EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAttack_Charged_Sting_Normal, pos);

    }

    private void ChargeAttackStab()
    {
        List<Collider> result = new List<Collider>();

        Vector3 forwardOffset = _player.transform.forward * 3f;
        Collider[] enemies = GetEnemyByOverlapBox(_player.transform.position + forwardOffset, _player.transform.rotation);
        
        if (_player.isChargingTripleSting)
        {
            Vector3 rightDir = _player.transform.eulerAngles + new Vector3(0, 15, 0);
            Vector3 rightOffset = (Quaternion.Euler(rightDir) * Vector3.forward) * 4;
            Collider[] rightEnemies = GetEnemyByOverlapBox(_player.transform.position + rightOffset, Quaternion.Euler(rightDir));

            Vector3 leftDir = _player.transform.eulerAngles - new Vector3(0, 15, 0);
            Vector3 leftOffset = (Quaternion.Euler(leftDir) * Vector3.forward) * 4;
            Collider[] leftEnemies = GetEnemyByOverlapBox(_player.transform.position + leftOffset, Quaternion.Euler(leftDir));

            result.AddRange(rightEnemies);
            result.AddRange(leftEnemies);            
        }
        result.AddRange(enemies);

        Attack(enemies.ToList());
    }

    protected override void HitEnemyAction(Brain enemy)
    {
        base.HitEnemyAction(enemy);

        CameraManager.Instance.ShakeCam(0.3f, 0.5f, 0.5f);
    }

    private void MoveToFront()
    {
        if (!_isStabMove)
        {
            _isStabMove = true;

            float dashDistance = _player.PlayerStatData.GetChargeAttackDistance();

            RaycastHit hit;
            Vector3 playerPos = _player.transform.position;

            if (Physics.Raycast(playerPos, _player.transform.forward, out hit, _player.PlayerStatData.GetChargeAttackDistance(), _player.whatIsWall))
            {
                dashDistance = hit.distance;
            }

            float stabDistance = _player.CurrentChargeTime * dashDistance;

            Vector3 moveDir = _player.transform.forward * stabDistance * 2;
            moveDir.y += _player.RigidbodyCompo.velocity.y;

            _player.SetVelocity(moveDir);
            
            //_player.StartCoroutine(MoveToFrontSmooth(_player.transform.position + (_player.transform.forward * stabDistance), _player.AnimatorCompo.speed));
        }
    }

    private IEnumerator MoveToFrontSmooth(Vector3 targetPos, float duration)
    {
        float delta = 0;
        float t = 0;

        while (t <= 1)
        {
            t = delta / duration;

            _player.transform.position = Vector3.LerpUnclamped(_player.transform.position, targetPos, t);
            delta += Time.deltaTime;

            if (_endTriggerCalled)
                break;

            yield return null;
        }
    }
}
