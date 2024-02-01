using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticleControl : MonoBehaviour
{
    private EnemyAI enemyAI;
    //public EnemyStats enemyStats;

    void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();

        //enemyStats._maxHp = 40;
        //enemyStats._currentHp = enemyStats._maxHp;
    }

    void Update()
    {
        ArticleOnDamage();
    }

    void ArticleOnDamage()
    {
        RaycastHit leftHit;
        RaycastHit rightHit;

        // 왼쪽 방향으로 레이 발사
        Vector3 leftDirection = GetDirectionFromLocal(Vector3.left);
        if (RaycastAndAttack(leftDirection, out leftHit))
        {
            Debug.Log("왼쪽 방향으로 공격");
            //leftHit.collider.GetComponent<Player>().TakeDamage(); // 플레이어에게 데미지 입히기 예시
        }

        // 오른쪽 방향으로 레이 발사
        Vector3 rightDirection = GetDirectionFromLocal(Vector3.right);
        if (RaycastAndAttack(rightDirection, out rightHit))
        {
            Debug.Log("오른쪽 방향으로 공격");
            //rightHit.collider.GetComponent<Player>().TakeDamage(); // 플레이어에게 데미지 입히기 예시
        }
    }

    Vector3 GetDirectionFromLocal(Vector3 localDirection)
    {
        // 오브젝트의 로컬 방향을 월드 좌표계로 변환
        return transform.TransformDirection(localDirection).normalized;
    }

    bool RaycastAndAttack(Vector3 direction, out RaycastHit hit)
    {
        // 레이 발사
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, LayerMask.GetMask("Player")))
        {
            FakePlayer player = hit.collider.GetComponent<FakePlayer>();
            return player != null;
        }

        return false;
    }
}
