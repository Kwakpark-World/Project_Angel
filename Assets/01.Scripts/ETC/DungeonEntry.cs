using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntry : MonoBehaviour
{
    [SerializeField] private Barrier _barrier;

    private readonly string _dungeonSceneName = "Dungeon_Map";

    private void Start()
    {
        _barrier.Show();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(_dungeonSceneName);
        }
    }
}
