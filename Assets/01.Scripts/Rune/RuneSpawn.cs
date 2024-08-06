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

    // Update is called once per frame
    void Update()
    {
        RuneSpawner();
    }

    void RuneSpawner()
    {
        if (spawnCount < _runeSpawnTrm.Length)
        {
            RuneManager.Instance.SpawnRune(_runeSpawnTrm[spawnCount].position);

            spawnCount++;
        }
    }
}
