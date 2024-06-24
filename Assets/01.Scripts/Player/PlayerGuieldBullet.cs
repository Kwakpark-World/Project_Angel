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

        // 현재 게임 오브젝트 자신을 리스트에서 제거
        enemies.RemoveAt(0);

        // 근처에 있는 적의 인덱스 찾기
        int targetEnemyIndex = enemies.FindIndex(enemy =>
            (transform.position - enemy.transform.position).sqrMagnitude > (nearbyRange * nearbyRange));

        // 근처에 적이 있을 경우
        if (targetEnemyIndex >= 0)
        {
            // 목표 적 설정
            Brain targetEnemy = enemies[targetEnemyIndex];
            // 목표 적을 향한 방향 계산
            Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
            // 목표 적을 향해 이동
            transform.position += direction * moveSpeed * Time.deltaTime;

            if(direction == transform.position)
            {
                PoolManager.Instance.Push(this);
            }
        }
    }
}
