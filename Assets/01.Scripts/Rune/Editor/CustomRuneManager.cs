using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RuneManager))]
public class CustomRuneManager : Editor
{
    private readonly string _soName = "RuneList";
    private readonly string _prefabPath = "Assets/11.SO/Rune";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RuneManager manager = target as RuneManager;

        if (GUILayout.Button("Generate RuneList"))
        {
            RuneListSO list = CreateAssetDatabase();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            manager.SetRuneList(list);
        }
    }

    private RuneListSO CreateAssetDatabase()
    {
        List<RuneDataSO> loadedList = new List<RuneDataSO>();

        string[] assetNames = AssetDatabase.FindAssets("", new[] { _prefabPath });

        foreach (string assetName in assetNames)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetName);
            RuneDataSO powerUp = AssetDatabase.LoadAssetAtPath<RuneDataSO>(path);
            if (powerUp == null) continue;
            loadedList.Add(powerUp);
        }
        Debug.Log($"Rune load complete [ count: {loadedList.Count} ]");

        RuneListSO powerUpList;
        powerUpList = AssetDatabase.LoadAssetAtPath<RuneListSO>($"{_prefabPath}/{_soName}.asset");
        string absoluteFilename = AssetDatabase.GenerateUniqueAssetPath($"{_prefabPath}/{_soName}.asset");
        if (powerUpList == null)
        {
            powerUpList = ScriptableObject.CreateInstance<RuneListSO>();
            powerUpList.list = loadedList;
            AssetDatabase.CreateAsset(powerUpList, absoluteFilename);
            Debug.Log($"Rune asset created at {absoluteFilename}");
        }
        else
        {
            powerUpList.list = loadedList;
            EditorUtility.SetDirty(powerUpList);
            Debug.Log($"Rune asset updated at {absoluteFilename}");
        }
        return powerUpList;
    }
}
