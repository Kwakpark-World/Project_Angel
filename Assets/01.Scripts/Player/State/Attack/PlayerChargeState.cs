using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChargeState : PlayerAttackState
{
    protected float _minChargeTime = 0.5f;
    protected float _maxChargeTime = 2f;

    public PlayerChargeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        _minChargeTime = player.PlayerStatData.GetMinChargingTime();
        _maxChargeTime = player.PlayerStatData.GetMaxChargingTime();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
