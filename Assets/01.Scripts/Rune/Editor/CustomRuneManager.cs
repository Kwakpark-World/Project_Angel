using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RuneManager))]
public class CustomRuneManager : Editor
{
    private readonly string _assetPath = "Assets/11.SO/Rune";
    private readonly string _soName = "RuneList";

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
        List<RuneDataSO> loadedRuneList = new List<RuneDataSO>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { _assetPath });

        foreach (string assetName in assetNames)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetName);
            RuneDataSO runeData = AssetDatabase.LoadAssetAtPath<RuneDataSO>(path);

            if (runeData == null) continue;

            loadedRuneList.Add(runeData);
        }

        Debug.Log($"Rune load complete [ count: {loadedRuneList.Count} ]");

        RuneListSO runeList = AssetDatabase.LoadAssetAtPath<RuneListSO>($"{_assetPath}/{_soName}.asset");
        string soFileName = AssetDatabase.GenerateUniqueAssetPath($"{_assetPath}/{_soName}.asset");

        if (runeList == null)
        {
            runeList = ScriptableObject.CreateInstance<RuneListSO>();
            runeList.list = loadedRuneList;
            AssetDatabase.CreateAsset(runeList, soFileName);

            Debug.Log($"Rune asset created at {soFileName}");
        }
        else
        {
            runeList.list = loadedRuneList;
            EditorUtility.SetDirty(runeList);

            Debug.Log($"Rune asset updated at {soFileName}");
        }

        return runeList;
    }
}
