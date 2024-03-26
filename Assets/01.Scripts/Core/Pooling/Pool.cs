using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : PoolableMono
{
    private Stack<T> _pool = new Stack<T>();
    private T _prefab;
    private PoolingType _poolingType;
    private Transform _parent;

    public Pool(T prefab, PoolingType poolingType, Transform parent, int itemAmount = 10)
    {
        _poolingType = poolingType;
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < itemAmount; ++i)
        {
            T item = GameObject.Instantiate(prefab, parent);
            item.poolingType = poolingType;
            item.gameObject.name = _poolingType.ToString();

            Push(item);
        }
    }

    public void Push(T item)
    {
        if (!Application.isPlaying)
        {
            return;
        }

        item.gameObject.SetActive(false);
        _pool.Push(item);
    }

    public T Pop()
    {
        T item;

        if (_pool.Count <= 0)
        {
            item = GameObject.Instantiate(_prefab, _parent);
            item.poolingType = _poolingType;
            item.gameObject.name = _poolingType.ToString();
        }
        else
        {
            item = _pool.Pop();

            item.gameObject.SetActive(true);
        }

        return item;
    }
}
