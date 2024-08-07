using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float actionRadius = 5f; //감지 범위

    private string targetTag = "EnemyMannequin";

    public bool allEnemysDead = false;

    [SerializeField] private GameObject[] _MannequinObject;
    [SerializeField] private List<EnemyMannequin> _mannequinList = new List<EnemyMannequin>();

    private HashSet<EnemyMannequin> _spawnedMannequins = new HashSet<EnemyMannequin>();

    private void Start()
    {
        _MannequinObject = GameObject.FindGameObjectsWithTag(targetTag);

        // 찾은 오브젝트에서 EnemyMannequin 컴포넌트를 가져와 리스트에 추가합니다.
        foreach (GameObject obj in _MannequinObject)
        {
            EnemyMannequin mannequin = obj.GetComponent<EnemyMannequin>();

            if (mannequin != null)
            {
                _mannequinList.Add(mannequin);
            }
        }
    }

    private void Update()
    {
        PerformActionInRadius();
        CheckAllEnemiesDead();
    }

    void PerformActionInRadius()
    {
        // 감지 범위 내에 있는 오브젝트를 찾습니다.
        foreach (GameObject obj in _MannequinObject)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if (distance <= actionRadius)
            {
                EnemyMannequin mannequin = obj.GetComponent<EnemyMannequin>();

                if (mannequin != null && !_spawnedMannequins.Contains(mannequin))
                {
                    mannequin.SpawnEnemy();
                    _spawnedMannequins.Add(mannequin); // 소환된 오브젝트를 추적 리스트에 추가
                }

                obj.SetActive(false);
            }
        }
    }

    void CheckAllEnemiesDead()
    {
        bool allEnemiesDead = true;

        foreach (GameObject obj in _MannequinObject)
        {
            if (obj.activeSelf)
            {
                allEnemiesDead = false;
                break;
            }
        }

        if (allEnemiesDead)
        {
            OnAllEnemiesDead();
        }
    }

    void OnAllEnemiesDead()
    {
        allEnemysDead = true;
    }

}
