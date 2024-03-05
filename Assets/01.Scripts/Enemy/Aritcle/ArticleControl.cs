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

        // ���� �������� ���� �߻�
        Vector3 leftDirection = GetDirectionFromLocal(Vector3.left);
        if (RaycastAndAttack(leftDirection, out leftHit))
        {
            Debug.Log("���� �������� ����");
            //leftHit.collider.GetComponent<Player>().TakeDamage(); // �÷��̾�� ������ ������ ����
        }

        // ������ �������� ���� �߻�
        Vector3 rightDirection = GetDirectionFromLocal(Vector3.right);
        if (RaycastAndAttack(rightDirection, out rightHit))
        {
            Debug.Log("������ �������� ����");
            //rightHit.collider.GetComponent<Player>().TakeDamage(); // �÷��̾�� ������ ������ ����
        }
    }

    Vector3 GetDirectionFromLocal(Vector3 localDirection)
    {
        // ������Ʈ�� ���� ������ ���� ��ǥ��� ��ȯ
        return transform.TransformDirection(localDirection).normalized;
    }

    bool RaycastAndAttack(Vector3 direction, out RaycastHit hit)
    {
        // ���� �߻�
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, LayerMask.GetMask("Player")))
        {
            FakePlayer player = hit.collider.GetComponent<FakePlayer>();
            return player != null;
        }

        return false;
    }
}
