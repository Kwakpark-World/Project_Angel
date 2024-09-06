using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSpawn : MonoBehaviour
{
    private int spawnCount = 0;

    [SerializeField]
    private Transform[] _runeSpawnTrm;

    // Start is called before the first frame update
    void Awake()
    {
        spawnCount = 0;
    }

    private void Start()
    {
        RuneSpawner(); 
    }

    void RuneSpawner()
    {
        if(_runeSpawnTrm != null)
        {
            for(spawnCount = 0; spawnCount <= _runeSpawnTrm.Length; spawnCount++)
            {
                if (spawnCount >= _runeSpawnTrm.Length)
                {
                    break;
                }

                RuneManager.Instance.SpawnRune(_runeSpawnTrm[spawnCount].position);

                Debug.Log("Rune Successfully Spanwed" + spawnCount);
            }
        }
    }
}
