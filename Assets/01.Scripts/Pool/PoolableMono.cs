using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableMono : MonoBehaviour
{
    public PoolingType poolingType;
    public bool sameLifeCycle;

    public virtual void InitializePoolingItem()
    {
        sameLifeCycle = false;
    }
}
