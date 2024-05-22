using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolListSO))]
public class CustomPoolListSO : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PoolListSO so = target as PoolListSO;

        if (GUILayout.Button("Set elements' name."))
        {
            for (int i = 0; i < so.poolList.Count; ++i)
            {
                so.poolList[i].name = so.poolList[i].type.ToString();
            }
        }

        if (GUILayout.Button("Sort elements by type."))
        {
            so.poolList = so.poolList.OrderBy(poolObject => poolObject.type).ToList();
        }

        if (GUILayout.Button("Fill empty item amounts."))
        {
            for (int i = 0; i < so.poolList.Count; ++i)
            {
                if (so.poolList[i].itemAmount <= 0)
                {
                    so.poolList[i].itemAmount = 10;
                }
            }
        }
    }
}
