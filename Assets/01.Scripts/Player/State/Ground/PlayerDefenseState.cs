using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseState : PlayerGroundState
{
    private float _defenseTimer;

    private ParticleSystem[] _thisParticles;

    private Color _normalColor = new Color(1, 0.9592881f, 0.4858491f, 1f);
    private Color _awakenColor = Color.red;

    public PlayerDefenseState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MeleeAttackEvent += EndDefenseToAttack;

        _player.IsDefense = true;
        _player.StopImmediately(false);

        
        _thisParticles = _player.effectParent.Find(_effectString).GetComponentsInChildren<ParticleSystem>();
        foreach (var particle in _thisParticles)
        {
            var main = particle.main;
            main.startColor = _player.IsAwakening ? _awakenColor : _normalColor;
            particle.Play();
        }
    
        
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
        foreach (var particle in _thisParticles)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        _defenseTimer = 0;
        _player.defensePrevTime = Time.time;

    }

    private void EndDefenseToAttack()
    {
        EndDefense();

        _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
    }
}
