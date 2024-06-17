using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateEnum
{
    Idle,
    Walk,
    Fall,
    NormalDash,
    AwakenDash,
    Defense,
    MeleeAttack,
    Awakening,
    NormalSlam,
    AwakenSlam,
    Charging,
    NormalChargeAttack,
    NormalChargeStabAttack,
    AwakenChargeAttack,
    Die,
}

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }
    public Dictionary<PlayerStateEnum, PlayerState> StateDictionary = new Dictionary<PlayerStateEnum, PlayerState>();

    private Player _player;

    public void Initialize(PlayerStateEnum startState, Player player)
    {
        _player = player;
        CurrentState = StateDictionary[startState];
        CurrentState.Enter();
    }

    public void AddState(PlayerStateEnum state, PlayerState playerState)
    {
        StateDictionary.Add(state, playerState);
    }

    public void ChangeState(PlayerStateEnum state)
    {
        if (_player.IsDie)
            return;
        if (_player.IsPlayerStop)
            return;

        CurrentState.Exit();
        CurrentState = StateDictionary[state];
        CurrentState.Enter();

        UIManager.Instance.PlayerHUDProperty.StartSkillCooldown(state);
    }

    public PlayerState GetState(PlayerStateEnum state)
    {
        return StateDictionary[state];
    }

    public bool CompareState(PlayerStateEnum state)
    {
        return (CurrentState == GetState(state));
    }
}