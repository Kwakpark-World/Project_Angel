using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolingListSO))]
public class CustomPoolingListSO : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PoolingListSO so = target as PoolingListSO;

        if (GUILayout.Button("Set elements' name."))
        {
            for (int i = 0; i < so.poolingList.Count; ++i)
            {
                so.poolingList[i].name = so.poolingList[i].type.ToString();
            }
        }
    }
}
