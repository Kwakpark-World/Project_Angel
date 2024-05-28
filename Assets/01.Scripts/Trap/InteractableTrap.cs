using AmplifyShaderEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableTrap : Trap
{
    public LayerMask hitableLayer;
    
    private bool _isDelay;

    protected Vector3 _attackCenter = Vector3.zero;
    protected Vector3 _attackHalfSize = Vector3.zero;
    protected Quaternion _attackRotation = Quaternion.identity;

    protected HashSet<Brain> _enemyDuplicateCheck = new HashSet<Brain>();
    protected HashSet<Player> _playerDuplicateCheck = new HashSet<Player>();

    protected float _prevRunTime;
    protected float _trapCoolTime = 5f;

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        SetPlayerRangeParameter();
    }

    protected override void EndTrap()
    {
        base.EndTrap();

        _enemyDuplicateCheck.Clear();
        _playerDuplicateCheck.Clear();
    }

    protected bool CoolCheck()
    {
        if (_prevRunTime + _trapCoolTime > Time.time) return false;

        _prevRunTime = Time.time;
        return true;
    }

    private Collider[] GetHitableObject()
    {
        return Physics.OverlapBox(_attackCenter, _attackHalfSize, _attackRotation, hitableLayer);
    }

    protected void AttackObject()
    {
        Collider[] hitableObj = GetHitableObject();

        foreach (var obj in hitableObj)
        {
            if (obj.TryGetComponent<Brain>(out Brain enemy))
            {
                if (_enemyDuplicateCheck.Add(enemy))
                {
                    Attack(enemy);
                }
            }
            else if (obj.TryGetComponent<Player>(out Player player))
            {
                if (_playerDuplicateCheck.Add(player))
                {
                    Attack(player);
                }
            }
        }
    }

    protected abstract void SetPlayerRangeParameter();

    public void StartDelayAction(float delayTime, Action todoAction)
    {
        StartCoroutine(DelayAction(delayTime, todoAction));
    }

    public void StartDelayAction(Action todoAction)
    {
        _isDelay = true;
        StartCoroutine(DelayAction(todoAction));
    }

    public void DelayActionStop()
    {
        _isDelay = false;
    }

    protected IEnumerator DelayAction(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    protected IEnumerator DelayAction(Action action)
    {
        while (_isDelay)
        {
            yield return null;
        }
        action?.Invoke();
    }


}
