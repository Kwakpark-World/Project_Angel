using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (_player.transform.root.gameObject.scene.name == "TutorialScene")
        {
            if (!_player._rollFirst)
            {
                _player._rollFirst = true;
                _player._tutorial.PlayerTutorialToggle(1);
            }
        }

        _player.PlayerInput.DashEvent += HandleRollOnceMoreEvent;
        
        CameraManager.Instance._currentCam.IsCamRotateStop = true;

        _player.IsDefense = true;
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.DashEvent -= HandleRollOnceMoreEvent;

        CameraManager.Instance._currentCam.IsCamRotateStop = false;
        _player.IsDefense = false;

        _player.enemyDashHitDuplicateChecker.Clear();
        _player.enemyKnockBackDuplicateChecker.Clear();

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_player.IsPlayerStop == PlayerControlEnum.Stop)
            return;

        List<Collider> enemies = GetEnemyToDash().ToList();

        if (_player.isRollKnockback)
        {
            KnockBack(enemies);
        }

        if (_player.isRollAttack)
        {
            Attack(enemies);
        }

        _player.AnimatorCompo.SetBool("RollOnceMore", _player.isOnRollOnceMore);
    }

    private void HandleRollOnceMoreEvent()
    {
        if (!_player.isRollOnceMore) return;
        if (_player.isOnRollOnceMore) return;
        if (!_actionTriggerCalled) return;

        _player.isOnRollOnceMore = true;
        _player.dashPrevTime = Time.time;

        if (!_player.isRollToDash)
            _player.StateMachine.ChangeState(PlayerStateEnum.NormalDash);
        else
            _player.StateMachine.ChangeState(PlayerStateEnum.AwakenDash);
    }

    private void KnockBack(List<Collider> enemies)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
            {
                if (_player.enemyKnockBackDuplicateChecker.Add(brain))
                {
                    Debug.Log("Knockback");
                    _player.StartCoroutine(brain.Knockback(_player.PlayerStatData.GetKnockbackPower()));
                }
            }
        }
    }

    ///
    protected Collider[] GetEnemyToDash()
    {
        Vector3 pos = _player.transform.forward + _player.playerCenter.position;

        Vector3 halfSize = Vector3.one * 0.5f;
        halfSize.y *= 2;

        return Physics.OverlapBox(pos, halfSize, _player.transform.rotation, _player.enemyLayer);
    }

    public void Attack(List<Collider> enemies)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
            {
                if (_player.enemyDashHitDuplicateChecker.Add(brain))
                {
                    Debug.Log("roll attacks");

                    brain.OnHit(_player.PlayerStatData.GetAttackPower(), true, false, 0);
                    
                    _player.CurrentAwakenGauge++;
                }
            }
        }
    }
}
