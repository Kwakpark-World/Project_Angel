using UnityEngine;

public class PlayerChargeState : PlayerState
{
    private float _clickTimer;

    private float _minChargeTime = 0.5f;
    private float _maxChargeTime = 2f;

    public PlayerChargeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _player.RotateToMousePos();

        _clickTimer = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _clickTimer = Mathf.Clamp(_clickTimer, 0f, _maxChargeTime);
        Debug.Log(_clickTimer);

        if (!_player.PlayerInput.isCharge)
        {
            if (_clickTimer < _minChargeTime)
                _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
            else
                _stateMachine.ChangeState(PlayerStateEnum.ChargeAttack);
        }
        else
        {
            _clickTimer += Time.deltaTime;

            if (_clickTimer >= _maxChargeTime)
            {
                _stateMachine.ChangeState(PlayerStateEnum.ChargeAttack);
            }
        }
        
    }
}
