using Autodesk.Fbx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AddressableManager : MonoSingleton<AddressableManager>
{
    private List<IEnumerator> _handles = new List<IEnumerator>();

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(IninAddressable()); // 혹시 모를 버그를 위해 초기화하는 함수.
    }

    private void OnDestroy()
    {
        if (_handles.Count > 0)
            ReleaseAsset();
    }


    public async Task<T> GetAssetToString<T>(string path)
    {
        T result = default(T);

        var loadAssets = Addressables.LoadAssetAsync<T>(path);
        await loadAssets.Task;

        result = loadAssets.Result;
        _handles.Add(loadAssets);

        return result;
    }

    public async Task<List<T>> GetAssetsByLabelToLabelName<T>(string labelName)
    {
        var result = new List<T>();

        var loadAssets = Addressables.LoadResourceLocationsAsync(labelName);
        await loadAssets.Task;

        var assetResults = loadAssets.Result;

        await LoadAssetByLabel<T>(assetResults, result);

        return result;
    }

    public async Task<List<T>> GetAssetsByLabelToRef<T>(AssetLabelReference assetRef)
    {
        List<T> result = new List<T>();

        var loadAssets = Addressables.LoadResourceLocationsAsync(assetRef.labelString);
        await loadAssets.Task;

        var assetResults = loadAssets.Result;

        await LoadAssetByLabel<T>(assetResults, result);

        return result;
    }

    private async Task LoadAssetByLabel<T>(IList<IResourceLocation> objs, List<T> list)
    {
        foreach (var item in objs)
        {
            var handle = Addressables.LoadAssetAsync<T>(item.PrimaryKey);
            await handle.Task;

            list.Add(handle.Result);
            _handles.Add(handle);
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
