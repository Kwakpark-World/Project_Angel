using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableMono : MonoBehaviour
{
    public PoolType poolType;
    [HideInInspector]
    public bool sameLifeCycle;

    public virtual void InitializePoolItem()
    {
        sameLifeCycle = false;
    }

    public void OnGameOver()
    {
        PoolManager.Instance.Push(this);
    }
}
