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

    public override void OnDie()
    {
        base.OnDie();

        GameManager.Instance.EnemyDieCount++;
    }
}
