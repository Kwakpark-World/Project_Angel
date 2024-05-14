using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseState : PlayerGroundState
{
    private float _defenseTimer;

    private ParticleSystem _thisParticle;

    public PlayerDefenseState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MeleeAttackEvent += EndDefenseToAttack;

        _player.IsDefense = true;
        _player.StopImmediately(false);

        
        _thisParticle = _player._effectParent.Find(_effectString).GetComponent<ParticleSystem>();
        _thisParticle.Play();
    
        _player.RotateToMousePos();
        
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.MeleeAttackEvent -= EndDefenseToAttack;

        _player.IsDefense = false;

        EndDefense();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _defenseTimer += Time.deltaTime;        

        if (!_player.PlayerInput.isDefense || _defenseTimer >= _player.defenseTime)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void EndDefense()
    {
        _thisParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        _defenseTimer = 0;
        _player.defensePrevTime = Time.time;

    }

    private void EndDefenseToAttack()
    {
        EndDefense();

        _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
    }
}
