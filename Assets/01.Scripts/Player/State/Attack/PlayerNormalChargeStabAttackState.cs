using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerNormalChargeStabAttackState : PlayerChargeState
{
    private bool _isStabMove;
    private bool _isEffectOn = false;

    public PlayerNormalChargeStabAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _isEffectOn = false;
        _isStabMove = false;
    }

    public override void Exit()
    {
        base.Exit();

        _player.enemyNormalHitDuplicateChecker.Clear();

        _player.ChargingGauge = 0;
        _player.AnimatorCompo.speed = 1;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        MoveToFront();

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                Vector3 pos = _player._weapon.transform.position;
                EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charged_Stab_Normal, pos);
            }
        }


        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void MoveToFront()
    {
        if (_actionTriggerCalled && !_isStabMove)
        {
            _isStabMove = true;

            float stabDistance = _player.ChargingGauge * _player.PlayerStatData.GetChargingAttackDistance();
            _player.SetVelocity(_player.transform.forward * stabDistance);
        }
    }
}
