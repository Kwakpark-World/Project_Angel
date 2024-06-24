using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGuieldBullet : PoolableMono
{
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float nearbyRange = 10f;

    private void Update()
    {
        List<Brain> enemies = FindObjectsOfType<Brain>()
            .OrderBy(enemy => (transform.position - enemy.transform.position).sqrMagnitude)
            .ToList();

        // ���� ���� ������Ʈ �ڽ��� ����Ʈ���� ����
        enemies.RemoveAt(0);

        // ��ó�� �ִ� ���� �ε��� ã��
        int targetEnemyIndex = enemies.FindIndex(enemy =>
            (transform.position - enemy.transform.position).sqrMagnitude > (nearbyRange * nearbyRange));

        // ��ó�� ���� ���� ���
        if (targetEnemyIndex >= 0)
        {
            // ��ǥ �� ����
            Brain targetEnemy = enemies[targetEnemyIndex];
            // ��ǥ ���� ���� ���� ���
            Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
            // ��ǥ ���� ���� �̵�
            transform.position += direction * moveSpeed * Time.deltaTime;

            if(direction == transform.position)
            {
                PoolManager.Instance.Push(this);
            }
        }
    }
}
