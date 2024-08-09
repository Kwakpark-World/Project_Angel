using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerNormalSlamState : PlayerAttackState
{
    private float _width = 5f;
    private float _height = 10f;
    private float _dist = 13f;
    private Vector3 _offset;

    private float _jumpForce = 10f;
    private float _dropForce = 44f;

    private bool _isEffectOn = false;
    
    public PlayerNormalSlamState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (_player.transform.root.gameObject.scene.name == "TutorialScene")
        {
            if (!_player._SlamFirst)
            {
                _player._SlamFirst = true;
                _player._tutorial.PlayerTutorialToggle(2);
            }
        }
        
        _player.StopImmediately(false);
        _isEffectOn = false;

        //JumpToFront();
    }

    public override void Exit()
    {
        base.Exit();

        _player.enemyNormalHitDuplicateChecker.Clear();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        
        //if (_actionTriggerCalled )
        //{
        //    AttackDrop();
        //}

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                CameraManager.Instance.ShakeCam(0.2f, 0.4f, 5f);
                SlamEffect();
            }
        }

        if (_endTriggerCalled )
        {
            if (_player.IsGroundDetected())
            {
                SlamAttack();
            } 
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    protected override void SetAttackSetting()
    {
        _hitDist = _dist;
        _hitHeight = _height;
        _hitWidth = _width;

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);

        _offset = _player.transform.forward * 5;
        //_offset.y += 1f;

        _attackOffset = _offset;
        _attackSize = size;
    }

    private void SlamAttack()
    {
        Collider[] enemies = GetEnemyByOverlapBox(_player.transform.position, _player.transform.rotation);

        Attack(enemies.ToList());
    }

    private void SlamEffect()
    {
        float yOffset = 0f;
        Vector3 rayPos = _player.transform.position;
        rayPos.y += 1f;
        RaycastHit hit;

        if (Physics.Raycast(rayPos, Vector3.down, out hit, 300f, _player.whatIsGround))
        {
            yOffset = hit.transform.position.y;
        }
        Vector3 pos = _player.transform.position + _attackOffset;
        pos.y = yOffset;

        EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAttack_Slam_Normal, pos);
        
        if (_player.isSlamEarthquake)
        {
            EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAttack_Slam_Awaken_0, pos);
            EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAttack_Slam_Awaken_1, pos);
        }

        if (_player.isSlamSixTimeSlam)
        {
            _player.StartCoroutine(IsSlamSixTimeSlam(rayPos));
        }
    }

    private void IsSlamSixTimeSlamAttack(int idx)
    {
        List<Collider> result = new List<Collider>();

        Collider[] enemies = GetEnemyByOverlapBox(_player.transform.position, _player.transform.rotation);

        if (_player.isChargingTripleSting)
        {
            Vector3 rightDir = _player.transform.eulerAngles + new Vector3(0, 15 * idx, 0);
            Collider[] rightEnemies = GetEnemyByOverlapBox(_player.transform.position, Quaternion.Euler(rightDir));

            Vector3 leftDir = _player.transform.eulerAngles - new Vector3(0, 15 * idx, 0);
            Collider[] leftEnemies = GetEnemyByOverlapBox(_player.transform.position, Quaternion.Euler(leftDir));

            result.AddRange(rightEnemies);
            result.AddRange(leftEnemies);
        }
        result.AddRange(enemies);

        Attack(enemies.ToList());
    }

    private IEnumerator IsSlamSixTimeSlam(Vector3 pos)
    {
        for (int i = 1; i <= 2; i++)
        {
            yield return new WaitForSeconds(1f);
            _player.enemyNormalHitDuplicateChecker.Clear();

            CameraManager.Instance.ShakeCam(0.2f, 0.4f, 5f);
            Vector3 rayPos = _player.transform.position;
            float yOffset = 0f;
            RaycastHit hit;

            rayPos.y += 1f;
            if (Physics.Raycast(rayPos, Vector3.down, out hit, 300f, _player.whatIsGround))
            {
                yOffset = hit.transform.position.y;
            }


            Vector3 leftPos = pos + _attackOffset;
            Vector3 rightPos = pos + _attackOffset;
            Vector3 leftDir = Vector3.zero;
            Vector3 rightDir = Vector3.zero;

            leftPos -= (_player.transform.forward * i * 0.5f);
            rightPos -= (_player.transform.forward * i * 0.5f);

            leftPos -= (_player.transform.right * i * 0.5f);
            rightPos += (_player.transform.right * i * 0.5f);

            leftPos.y = yOffset;
            rightPos.y = yOffset;

            leftDir.x = -90f; // default Effect angle
            leftDir.z = _player.transform.eulerAngles.y - (15 * i);

            rightDir.x = -90f; // default Effect angle
            rightDir.z = _player.transform.eulerAngles.y + (15 * i);

            Quaternion leftRot = Quaternion.Euler(leftDir);
            Quaternion rightRot = Quaternion.Euler(rightDir);

            
            PoolableMono leftParticle = EffectManager.Instance.PlayAndGetEffect(PoolType.Effect_PlayerAttack_Slam_Normal, leftPos);
            PoolableMono rightParticle = EffectManager.Instance.PlayAndGetEffect(PoolType.Effect_PlayerAttack_Slam_Normal, rightPos);
            
            if (_player.isSlamEarthquake)
            {
                PoolableMono learthquakeParticle_0 = EffectManager.Instance.PlayAndGetEffect(PoolType.Effect_PlayerAttack_Slam_Awaken_0, leftPos);
                PoolableMono learthquakeParticle_1 = EffectManager.Instance.PlayAndGetEffect(PoolType.Effect_PlayerAttack_Slam_Awaken_1, leftPos);

                PoolableMono rearthquakeParticle_0 = EffectManager.Instance.PlayAndGetEffect(PoolType.Effect_PlayerAttack_Slam_Awaken_0, rightPos);
                PoolableMono rearthquakeParticle_1 = EffectManager.Instance.PlayAndGetEffect(PoolType.Effect_PlayerAttack_Slam_Awaken_1, rightPos);

                Vector3 lRot = Vector3.zero;
                Vector3 rRot = Vector3.zero;

                lRot.y = leftDir.z;
                rRot.y = rightDir.z;

                Quaternion leftEarthquakeRot = Quaternion.Euler(lRot);
                Quaternion rightEarthquakeRot = Quaternion.Euler(rRot);

                learthquakeParticle_0.transform.rotation = leftEarthquakeRot;
                learthquakeParticle_1.transform.rotation = leftEarthquakeRot;
                rearthquakeParticle_0.transform.rotation = rightEarthquakeRot;
                rearthquakeParticle_1.transform.rotation = rightEarthquakeRot;
            }

            leftParticle.transform.rotation = leftRot;
            rightParticle.transform.rotation = rightRot;

            IsSlamSixTimeSlamAttack(i);
        }
    }
}
