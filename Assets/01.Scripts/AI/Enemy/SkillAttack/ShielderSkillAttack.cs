using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderSkillAttack : MonoBehaviour
{
    public float shieldDuration = 3f; // 쉴드 지속 시간
    public LayerMask enemyLayer; // 적의 레이어
    public int maxEnemiesToShield = 5; // 쉴드를 부여할 최대 적 수
    public float shieldRange = 10f; // 쉴드를 부여할 최대 거리

    // Start is called before the first frame update
    void Start()
    {
        // 3초마다 지속되는 쉴드를 적에게 적용
        StartCoroutine(ApplyShieldToEnemies());
    }

    IEnumerator ApplyShieldToEnemies()
    {
        // 3초 간격으로 반복
        while (true)
        {
            yield return new WaitForSeconds(shieldDuration);

            // 쉴드 효과를 주는 함수 호출
            ApplyShield();
        }
    }

    void ApplyShield()
    {
        // 레이캐스트를 통해 가장 가까운 5명의 적을 찾음
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.forward, shieldRange, enemyLayer);

        // 쉴드를 부여할 최대 적 수만큼만 적용
        int enemiesShielded = 0;
        foreach (RaycastHit hit in hits)
        {
            if (enemiesShielded >= maxEnemiesToShield)
                break;

            GameObject enemy = hit.collider.gameObject;

            // 적이 쉴드를 이미 가지고 있는지 확인
           /* if (!enemy.GetComponent<ShieldEffect>())
            {
                // 적에게 쉴드 효과 추가
                ShieldEffect shieldEffect = enemy.AddComponent<ShieldEffect>();
                // 쉴드 지속 시간 설정
                shieldEffect.duration = shieldDuration;

                enemiesShielded++;
            }*/
        }
    }
}
