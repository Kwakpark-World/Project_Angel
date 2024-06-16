using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNormalChargeStabAttackState : PlayerChargeState
{
    private float _width = 6f;
    private float _height = 8f;
    private float _dist = 12f;

    private bool _isStabMove;
    private bool _isEffectOn = false;

    private ParticleSystem _thisParticle;


    public PlayerNormalChargeStabAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _isEffectOn = false;
        _isStabMove = false;
        _thisParticle = _player.effectParent.Find(_effectString).GetComponent<ParticleSystem>();
    }

    public override void Exit()
    {
        base.Exit();

        _player.enemyNormalHitDuplicateChecker.Clear();

        _player.ChargingGauge = 0;
        _player.AnimatorCompo.speed = 1;

        _thisParticle.Stop();

    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_isHitAbleTriggerCalled)
        {
            ChargeAttackStab();
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

        _attackOffset = _player.transform.forward * 3f;
        _attackSize = size;
    }

    private void ChargeAttackStabEffect()
    {
        
        Vector3 pos = _weaponRB.transform.position;
        _thisParticle.transform.position = pos;

        _thisParticle.Play();
        //Vector3 pos = _player._weapon.transform.position;
        //EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAttack_Charged_Sting_Normal, pos);

    }

    private void ChargeAttackStab()
    {
        Collider[] enemies = GetEnemyByOverlapBox(_player.transform.position, _player.transform.rotation);

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

            float dashDistance = _player.PlayerStatData.GetChargingAttackDistance();

            RaycastHit hit;
            Vector3 playerPos = _player.transform.position;

            if (Physics.Raycast(playerPos, _player.transform.forward, out hit, _player.PlayerStatData.GetChargingAttackDistance(), _player.whatIsWall))
            {
                dashDistance = hit.distance;
            }

            Vector3 mouseTarget = _player.MousePosInWorld;
            float mousePosDist = Vector3.Distance(playerPos, _player.MousePosInWorld);

            mouseTarget.y = _player.transform.position.y + 0.1f;
            playerPos.y += 0.1f;
            if (Physics.Raycast(playerPos, mouseTarget, out hit, _player.PlayerStatData.GetChargingAttackDistance(), _player.whatIsWall))
            {
                mousePosDist = hit.distance - 2;
            }

            dashDistance = Mathf.Min(dashDistance, mousePosDist);

            float stabDistance = _player.ChargingGauge * dashDistance;
            
            _player.SetVelocity(_player.transform.forward * stabDistance * 5);

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
