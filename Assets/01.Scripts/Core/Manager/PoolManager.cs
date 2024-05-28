using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<PoolType, Pool<PoolableMono>> _pools = new Dictionary<PoolType, Pool<PoolableMono>>();
    [SerializeField]
    private PoolListSO _poolingList;

    protected override void Awake()
    {
        base.Awake();

        foreach (var poolingObject in _poolingList.poolList)
        {
            CreatePool(poolingObject.prefab, poolingObject.type, poolingObject.itemAmount);
        }
    }

    public void CreatePool(PoolableMono prefab, PoolType poolingType, int itemAmount = 10)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, poolingType, transform, itemAmount);

        _pools.Add(poolingType, pool);
    }

    public void Push(PoolableMono item, bool resetParent = false)
    {
        item.sameLifeCycle = false;

        if (resetParent)
        {
            item.transform.parent = transform;
        }

        _pools[item.poolType].Push(item);
    }

    public async void Push(PoolableMono item, float secondsDelay, bool resetParent = false)
    {
        item.sameLifeCycle = true;

        await Task.Delay((int)(secondsDelay * 1000));

        if (!item.sameLifeCycle)
        {
            return;
        }

        if (resetParent)
        {
            item.transform.parent = transform;
        }

        _pools[item.poolType].Push(item);
    }

    public PoolableMono Pop(PoolType poolingType, Vector3 position)
    {
        if (!_pools.ContainsKey(poolingType))
        {
            Debug.LogError("Pool object doesn't exist on pool.");

            return null;
        }

        PoolableMono item = _pools[poolingType].Pop();
        item.transform.position = position;

        item.InitializePoolItem();

        return item;
    }

    public PoolableMono Pop(PoolType poolingType, Vector3 position, Transform parent)
    {
        if (!_pools.ContainsKey(poolingType))
        {
            Debug.LogError("Pool object doesn't exist on pool.");

            return null;
        }

        PoolableMono item = _pools[poolingType].Pop();
        item.transform.position = position;
        item.transform.parent = parent;

        item.InitializePoolItem();

        return item;
    }
}
