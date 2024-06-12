using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<PoolType, Pool<PoolableMono>> _pools = new Dictionary<PoolType, Pool<PoolableMono>>();
    private Transform _healthBarCanvas;
    [SerializeField]
    private LayerMask _uiLayer;
    [SerializeField]
    private PoolListSO _poolList;

    protected override void Awake()
    {
        base.Awake();

        _healthBarCanvas = transform.Find("Canvas_HealthBar");
        _healthBarCanvas.GetComponent<Canvas>().worldCamera = Camera.main;

        foreach (var poolObject in _poolList.poolList)
        {
            CreatePool(poolObject.prefab, poolObject.type, poolObject.itemAmount);
        }
    }

    public void CreatePool(PoolableMono prefab, PoolType poolType, int itemAmount = 10)
    {
        Pool<PoolableMono> pool;

        if ((1 << prefab.gameObject.layer & _uiLayer) == 0)
        {
            pool = new Pool<PoolableMono>(prefab, poolType, transform, itemAmount);
        }
        else
        {
            pool = new Pool<PoolableMono>(prefab, poolType, _healthBarCanvas, itemAmount);
        }

        _pools.Add(poolType, pool);
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

    public PoolableMono Pop(PoolType poolType, Vector3 position)
    {
        if (!_pools.ContainsKey(poolType))
        {
            Debug.LogError("Pool object doesn't exist on pool.");

            return null;
        }

        PoolableMono item = _pools[poolType].Pop();
        item.transform.position = position;

        item.InitializePoolItem();

        return item;
    }

    public PoolableMono Pop(PoolType poolType, Vector3 position, Transform parent)
    {
        if (!_pools.ContainsKey(poolType))
        {
            Debug.LogError("Pool object doesn't exist on pool.");

            return null;
        }

        PoolableMono item = _pools[poolType].Pop();
        item.transform.position = position;
        item.transform.parent = parent;

        item.InitializePoolItem();

        return item;
    }
}
