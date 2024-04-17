using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillUpgradeSO))]
public class CustomSkillUpgradeSO : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SkillUpgradeSO so = (SkillUpgradeSO)target;
        
        DrawMethod(so);
    }

    private void DrawMethod(SkillUpgradeSO so)
    {
        GUIContent methodLabel = new GUIContent("Target Method");
        string[] names = so.methodList.Select(x => x.Name).ToArray();

        so.selectedMethodIndex = EditorGUILayout.Popup(
                                methodLabel, so.selectedMethodIndex, names);

        GUIContent killMethodLabel = new GUIContent("Kill Method");
        string[] killNames = so.killMethodList.Select(x => x.Name).ToArray();

        so.selectedMethodIndex = EditorGUILayout.Popup(
                                killMethodLabel, so.selectedKillMethodIndex, killNames);

        so.paramStr = EditorGUILayout.TextField("Method Param", so.paramStr);
    }
}
