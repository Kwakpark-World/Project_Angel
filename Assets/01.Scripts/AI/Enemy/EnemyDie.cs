using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    public bool enable;

    private void Start()
    {
        // �±׸� ����Ͽ� EnemySpawner�� ã���ϴ�.
        GameObject spawnerObject = GameObject.FindWithTag("EnemySpawner");

        if (spawnerObject != null)
        {
            enemySpawner = spawnerObject.GetComponent<EnemySpawner>();
            if (enemySpawner == null)
            {
                Debug.LogError("EnemySpawner component not found on the object with EnemySpawner tag.");
            }
            else
            {
                Debug.Log("EnemySpawner component found and assigned.");
            }
        }
        else
        {
            Debug.LogError("No GameObject found with the EnemySpawner tag.");
        }
    }

    private void OnEnable()
    {
        enable = true;
    }

    private void OnDisable()
    {
        // ���� ��Ȱ��ȭ�� �� EnemySpawner�� EnemyDead �޼��带 ȣ���մϴ�.
        if (enable == true)
        {
            if (GameManager.Instance.IsGameStart)
            {
                Debug.Log("Calling EnemyDead method.");
                enemySpawner.EnemyDead();
                enable = false;
            }
        }

        else
        {
            Debug.LogError("EnemySpawner is null, cannot call EnemyDead method.");
        }
    }
}
