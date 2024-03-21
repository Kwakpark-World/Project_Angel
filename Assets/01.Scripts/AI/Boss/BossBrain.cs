using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBrain : Brain
{
    private int _patternAmount;
    private float _patternTimer;

    protected override void Update()
    {
        base.Update();

        if (Time.time > _patternTimer + treeRunner.tree.blackboard.nextPatternCooldown)
        {
            _patternTimer = Time.time;
            treeRunner.tree.blackboard.selectedPattern = Random.Range(0, _patternAmount) + 1;
        }
    }

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        treeRunner.tree.blackboard.nextPatternCooldown = EnemyStatData.GetInitialPatternCooldown();
        _patternTimer = Time.time;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _patternAmount = EnemyStatData.GetPatternAmount();
    }

    public override void OnHit()
    {
        if (EnemyStatData.GetCurrentHealth() <= 0f)
        {
            // Set boss to dead.
            OnDie();
        }
    }

    public override void OnDie()
    {
        AnimatorCompo.SetBoolEnable("isDie");
    }
}
