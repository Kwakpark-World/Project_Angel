using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateEnum
{
    Idle,
    Move,
    Fall,
    Dash,
    MeleeAttack,
    QSkill,
    ESkill,
    Defense,
    Die
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
        // 플레이어가  맞고 있거나 뭔가 일이 있어서 상태 전환하지 못하는 경우
        if (_player.IsDie)
        {
            return;
        }

        CurrentState.Exit();
        CurrentState = StateDictionary[state];
        CurrentState.Enter();
        Debug.Log($"Change State {CurrentState}");
    }

    public PlayerState GetState(PlayerStateEnum state)
    {
        return StateDictionary[state];
    }
}