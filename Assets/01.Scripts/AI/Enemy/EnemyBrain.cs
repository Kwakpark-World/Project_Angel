using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : Brain
{
    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        treeRunner.tree.blackboard.home = transform.position;
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Debug
        treeRunner.tree.blackboard.home = transform.position;
    }

    public override void OnHit()
    {
        if (EnemyStatistic.GetCurrentHealth() <= 0f)
        {
            OnDie();
        }
    }

    public override void OnDie()
    {
        AnimatorCompo.SetBoolEnable("isDie");
    }
}
