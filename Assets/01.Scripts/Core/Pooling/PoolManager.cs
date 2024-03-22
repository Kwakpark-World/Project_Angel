using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance = null;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PoolManager>();

            if (_instance == null)
                Debug.LogError("PoolManager Component is null");

            return _instance;
        }
    }
    private Dictionary<PoolingType, Pool<PoolableMono>> _pools = new Dictionary<PoolingType, Pool<PoolableMono>>();
    [SerializeField]
    private PoolingListSO _poolingList;

    private void Awake()
    {
        foreach (var poolingObject in _poolingList.poolingList)
        {
            CreatePool(poolingObject.prefab, poolingObject.type, poolingObject.itemAmount);
        }
    }

    public void CreatePool(PoolableMono prefab, PoolingType poolingType, int itemAmount = 10)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, poolingType, transform, itemAmount);

        _pools.Add(poolingType, pool);
    }

    public void Push(PoolableMono item, bool resetParent =  false)
    {
        if (resetParent)
        {
            item.transform.parent = transform;
        }

        _pools[item.poolingType].Push(item);
    }

    public async void Push(PoolableMono item, int secondsDelay, bool resetParent = false)
    {
        await Task.Delay(secondsDelay * 1000);

        if (resetParent)
        {
            item.transform.parent = transform;
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

    public PoolableMono Pop(PoolingType poolingType, Vector3 position)
    {
        if (!_pools.ContainsKey(poolingType))
        {
            Debug.LogError("Pooling object doesn't exist on pool.");

            return null;
        }

        PoolableMono item = _pools[poolingType].Pop();
        item.transform.position = position;

        item.InitializePoolingItem();

        return item;
    }
}
