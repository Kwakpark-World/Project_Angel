using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    private Dictionary<PoolingType, Pool<PoolableMono>> _pools = new Dictionary<PoolingType, Pool<PoolableMono>>();
    [SerializeField]
    private PoolingListSO _poolingList;
    private Transform _parent;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        
        foreach (var poolingObject in _poolingList.poolingList)
        {
            CreatePool(poolingObject.prefab, poolingObject.type, poolingObject.itemAmount);
        }
    }

    public PoolManager(Transform parent)
    {
        _parent = parent;
    }

    public void CreatePool(PoolableMono prefab, PoolingType poolingType, int itemAmount = 10)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, poolingType, _parent, itemAmount);

        _pools.Add(poolingType, pool);
    }

    public void Push(PoolableMono item, bool resetParent =  false)
    {
        if (resetParent)
        {
            item.transform.parent = _parent;
        }

        _pools[item.poolingType].Push(item);
    }

    public PoolableMono Pop(PoolingType poolingType)
    {
        if (!_pools.ContainsKey(poolingType))
        {
            Debug.LogError("Pooling object doesn't exist on pool.");

            return null;
        }

        PoolableMono item = _pools[poolingType].Pop();

        item.InitializePoolingItem();

        return item;
    }
}