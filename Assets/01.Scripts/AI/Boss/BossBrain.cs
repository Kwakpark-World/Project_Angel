using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBrain : Brain
{
    #region Debug
    [Header("Debug statistics")]
    public int patternAmount;
    public float initialPatternCooldown = 10f;
    private float _patternTimer;
    #endregion

    protected override void Update()
    {
        if (Time.time > _patternTimer + treeRunner.tree.blackboard.nextPatternCooldown)
        {
            _patternTimer = Time.time;
            treeRunner.tree.blackboard.selectedPattern = Random.Range(0, patternAmount) + 1;
        }
    }

    protected override void Initialize()
    {
        currentHitPoint = hitPoint;
        normalAttackTimer = Time.time;
        treeRunner.tree.blackboard.nextPatternCooldown = initialPatternCooldown;
        _patternTimer = Time.time;
    }

    public override void OnHit(float damage)
    {
        currentHitPoint -= damage;

        if (currentHitPoint <= 0f)
        {
            currentHitPoint = 0f;

            // Set boss to dead.
            OnDie();
        }
    }

    public override void OnDie()
    {

    }
}
