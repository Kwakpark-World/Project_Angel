using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBrain : MonoBehaviour
{
    // Control statistics here using scriptable object and so on.

    public BTVisual.BehaviourTreeRunner treeRunner;
    public int patternAmount;
    public float initiailPatternCooldown = 10f;
    private float _patternTimer;
    public float hitPoint;
    private float _currentHitPoint;
    public float CurrentHitPoint
    {
        get => _currentHitPoint;
        set => _currentHitPoint = value;
    }
    public float normalAttackDelay = 3f;
    private float _normalAttackTimer;
    public float NormalAttackTimer
    {
        get => _normalAttackTimer;
        set => _normalAttackTimer = value;
    }

    private void Start()
    {
        treeRunner.tree.blackboard.nextPatternCooldown = initiailPatternCooldown;
        _patternTimer = Time.time;
        _currentHitPoint = hitPoint;
        _normalAttackTimer = Time.time;
    }

    private void Update()
    {
        if (Time.time > _patternTimer + treeRunner.tree.blackboard.nextPatternCooldown)
        {
            _patternTimer = Time.time;
            treeRunner.tree.blackboard.selectedPattern = Random.Range(0, patternAmount) + 1;
        }
    }

    public virtual void OnDie()
    {

    }

    public void OnHit(float damage)
    {
        _currentHitPoint -= damage;

        if (_currentHitPoint <= 0f)
        {
            _currentHitPoint = 0f;

            // Set boss to dead.
            OnDie();
        }
    }
}
