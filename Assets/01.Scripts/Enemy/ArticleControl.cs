using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticleControl : MonoBehaviour
{
    private EnemyAI enemyAI;

    void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();

        enemyAI.Enemy_MaxHp = 40;
        enemyAI.Enemy_CurrentHp = enemyAI.Enemy_MaxHp;
    }

    void Update()
    {
        PerformRaycast(Vector3.forward, "����");
    }

    void PerformRaycast(Vector3 direction, string directionName)
    {
        RaycastHit hit;

        if (RaycastAndAttack(direction, out hit))
        {
            Debug.Log(directionName + " �������� ����");
            // hit.collider.GetComponent<Player>().TakeDamage(); // �÷��̾�� ������ ������ ����
        }
    }

    bool RaycastAndAttack(Vector3 direction, out RaycastHit hit)
    {
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, LayerMask.GetMask("Player")))
        {
            FakePlayer player = hit.collider.GetComponent<FakePlayer>();
            return player != null;
        }

        return false;
    }
}
