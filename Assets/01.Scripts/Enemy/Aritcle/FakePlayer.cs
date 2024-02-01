using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    private void Start()
    {
        // 예시로 FakePlayer가 생성될 때 적의 데미지를 받도록 함
        EnemyAI enemyAI = FindObjectOfType<EnemyAI>(); // 적의 AI 스크립트를 찾아옴
    }

    public void TakeDamage(float damage)
    {
        // 여기에 데미지를 받는 로직 추가
        // 이 예시에서는 단순히 데미지만 출력
        Debug.Log("FakePlayer이 " + damage + "의 데미지를 받았습니다.");

        // 여기에서 필요한 추가적인 로직을 구현할 수 있습니다.
    }
}
