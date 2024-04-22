using Autodesk.Fbx;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AddressableManager : MonoSingleton<AddressableManager>
{
    public AssetLabelReference _playerRef;

    private List<IEnumerator> _handles = new List<IEnumerator>();

    private void Start()
    {
        StartCoroutine(IninAddressable()); // 혹시 모를 버그를 위해 초기화하는 함수.
    }

    private void OnDestroy()
    {
        if (_handles.Count > 0)
            ReleaseAsset();
    }

    public T GetAssetToString<T>(string path)
    {
        T result = default(T);
        Addressables.LoadAssetAsync<T>(path).Completed += (AsyncOperationHandle<T> obj) =>
        {
            result = obj.Result;
            _handles.Add(obj);
        };

        return result;
    }

    public List<T> GetAssetByLabelToString<T>(string labelName)
    {
        List<T> result = new List<T>();

        Addressables.LoadResourceLocationsAsync(labelName).Completed += (obj) => LoadAssetByLabel(obj, result);
        Addressables.LoadResourceLocationsAsync(labelName).Completed -= (obj) => LoadAssetByLabel(obj, result);

        return result;
    }

    public List<T> GetAssetByLabelToRef<T>(AssetLabelReference assetRef)
    {
        List<T> result = new List<T>();

        Addressables.LoadResourceLocationsAsync(assetRef.labelString).Completed += (obj) => LoadAssetByLabel(obj, result);
        Addressables.LoadResourceLocationsAsync(assetRef.labelString).Completed -= (obj) => LoadAssetByLabel(obj, result);
        
        return result;
    }

    private void LoadAssetByLabel<T>(AsyncOperationHandle<IList<IResourceLocation>> obj, List<T> list)
    {
        foreach (var item in obj.Result)
        {
            Addressables.LoadAssetAsync<T>(item.PrimaryKey).Completed += (value) =>
            {
                list.Add(value.Result);
                _handles.Add(value);
            };
        }

    }

    private void ReleaseAsset()
    {
        foreach (var item in _handles)
        {
            Addressables.Release(item);
        }
    }

    private IEnumerator IninAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }
}
