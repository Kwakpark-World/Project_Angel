using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float actionRadius = 5f; //���� ����

    private string targetTag = "EnemyMannequin";

    public bool allEnemysDead = false;

    [SerializeField] private GameObject[] _MannequinObject;
    [SerializeField] private List<EnemyMannequin> _mannequinList = new List<EnemyMannequin>();

    private HashSet<EnemyMannequin> _spawnedMannequins = new HashSet<EnemyMannequin>();

    private void Start()
    {
        _MannequinObject = GameObject.FindGameObjectsWithTag(targetTag);

        // ã�� ������Ʈ���� EnemyMannequin ������Ʈ�� ������ ����Ʈ�� �߰��մϴ�.
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
        // ���� ���� ���� �ִ� ������Ʈ�� ã���ϴ�.
        foreach (GameObject obj in _MannequinObject)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if (distance <= actionRadius)
            {
                EnemyMannequin mannequin = obj.GetComponent<EnemyMannequin>();

                if (mannequin != null && !_spawnedMannequins.Contains(mannequin))
                {
                    mannequin.SpawnEnemy();
                    _spawnedMannequins.Add(mannequin); // ��ȯ�� ������Ʈ�� ���� ����Ʈ�� �߰�
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
