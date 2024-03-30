using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeSkillEffectSO))]
public class CustomUpgradeSkillEffectSO : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UpgradeSkillEffectSO so = (UpgradeSkillEffectSO)target;
        
        DrawMethod(so);
    }

    private void DrawMethod(UpgradeSkillEffectSO so)
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
