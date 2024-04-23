using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MaterialManager : MonoSingleton<MaterialManager>
{
    private Dictionary<string, List<Material>> _materials = new Dictionary<string, List<Material>>(); // Label, (matName, mat)

    private async void Start()
    {
        await LoadMaterial();
    }

    private async Task LoadMaterial()
    {
        List<string> labelList = AddressableAssetSettingsDefaultObject.Settings.GetLabels();

        foreach (var label in labelList)
        {
            List<Material> _getMats = new List<Material>();
            _getMats = await AddressableManager.Instance.GetAssetsByLabelToLabelName<Material>(label);

            _materials.Add(label, _getMats);
        }
    }

    public List<Material> GetMaterial(string labelName)
    {
        return _materials[labelName];
    }

    public Material GetMaterial(string labelName, string matName)
    {
        int cnt = 0;
        Material result = null;

        foreach(var item in _materials[labelName])
        {
            if (item.name == matName) 
            {
                cnt++;
                result = item;
            }
        }

        if (cnt > 1)
        {
            Debug.LogError($"This Label Contain Same Name Material : {matName}");
            return null;
        }

        return result;
    }

}
