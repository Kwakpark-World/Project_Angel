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
    private float raycastLength = 1f;  // 레이캐스트 길이 증가

    public LayerMask enemyLayer;  // 적 레이어 마스크

    private void Update()
    {
        // 모든 적들을 거리순으로 정렬하여 가져오기
        List<Brain> enemies = FindObjectsOfType<Brain>()
            .OrderBy(enemy => (transform.position - enemy.transform.position).sqrMagnitude)
            .ToList();

        // 현재 게임 오브젝트 자신을 리스트에서 제거
        if (enemies.Count > 0)
        {
            enemies.RemoveAt(0);
        }

        // 근처에 있는 적의 인덱스 찾기
        int targetEnemyIndex = enemies.FindIndex(enemy =>
            (transform.position - enemy.transform.position).sqrMagnitude < (nearbyRange * nearbyRange));

        // 근처에 적이 있을 경우
        if (targetEnemyIndex >= 0)
        {
            // 목표 적 설정
            Brain targetEnemy = enemies[targetEnemyIndex];
            // 목표 적을 향한 방향 계산
            Vector3 direction = (targetEnemy.enemyCenter.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // 디버그 레이 추가
            Debug.DrawRay(transform.position, direction * raycastLength, Color.red);

            // 적과의 충돌을 감지하기 위한 레이캐스트
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
