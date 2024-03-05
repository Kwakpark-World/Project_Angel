using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] EnemyWeaponPrefabs;

    List<GameObject>[] EnemyWeaponPools;

    private void Awake()
    {
        EnemyWeaponPools = new List<GameObject>[EnemyWeaponPrefabs.Length];

        for(int i =0; i < EnemyWeaponPools.Length; i++)
        {
            EnemyWeaponPools[i] = new List<GameObject>();
        }
    }

    public GameObject GetEnemyArrow(int index)
    {
        GameObject selectedEnemyArrow = null;

        foreach (GameObject item in EnemyWeaponPools[index])
        {
            if(!item.activeSelf)
            {
                selectedEnemyArrow = item;
                selectedEnemyArrow.SetActive(true);
                break;
            }
        }

        if(!selectedEnemyArrow)
        {
            selectedEnemyArrow = Instantiate(EnemyWeaponPrefabs[index], transform);
            EnemyWeaponPools[index].Add(selectedEnemyArrow);
        }

        return selectedEnemyArrow;
    }
}
