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

    [SerializeField]
    private float raycastLength = 1f;  // ����ĳ��Ʈ ���� ����

    public LayerMask enemyLayer;  // �� ���̾� ����ũ

    private void Update()
    {
        // ��� ������ �Ÿ������� �����Ͽ� ��������
        List<Brain> enemies = FindObjectsOfType<Brain>()
            .OrderBy(enemy => (transform.position - enemy.transform.position).sqrMagnitude)
            .ToList();

        // ���� ���� ������Ʈ �ڽ��� ����Ʈ���� ����
        if (enemies.Count > 0)
        {
            enemies.RemoveAt(0);
        }

        // ��ó�� �ִ� ���� �ε��� ã��
        int targetEnemyIndex = enemies.FindIndex(enemy =>
            (transform.position - enemy.transform.position).sqrMagnitude < (nearbyRange * nearbyRange));

        // ��ó�� ���� ���� ���
        if (targetEnemyIndex >= 0)
        {
            // ��ǥ �� ����
            Brain targetEnemy = enemies[targetEnemyIndex];
            // ��ǥ ���� ���� ���� ���
            Vector3 direction = (targetEnemy.enemyCenter.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // ����� ���� �߰�
            Debug.DrawRay(transform.position, direction * raycastLength, Color.red);

            // ������ �浹�� �����ϱ� ���� ����ĳ��Ʈ
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, raycastLength, enemyLayer))
            {
                OnDamage(targetEnemy);
                PoolManager.Instance.Push(this);
            }
        }
    }

    private void OnDamage(Brain enemy)
    {
        enemy.OnHit(5);
    }
}
