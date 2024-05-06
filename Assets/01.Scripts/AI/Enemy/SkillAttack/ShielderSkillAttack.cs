using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderSkillAttack : MonoBehaviour
{
    public float shieldDuration = 3f; // ���� ���� �ð�
    public LayerMask enemyLayer; // ���� ���̾�
    public int maxEnemiesToShield = 5; // ���带 �ο��� �ִ� �� ��
    public float shieldRange = 10f; // ���带 �ο��� �ִ� �Ÿ�

    // Start is called before the first frame update
    void Start()
    {
        // 3�ʸ��� ���ӵǴ� ���带 ������ ����
        StartCoroutine(ApplyShieldToEnemies());
    }

    IEnumerator ApplyShieldToEnemies()
    {
        // 3�� �������� �ݺ�
        while (true)
        {
            yield return new WaitForSeconds(shieldDuration);

            // ���� ȿ���� �ִ� �Լ� ȣ��
            ApplyShield();
        }
    }

    void ApplyShield()
    {
        // ����ĳ��Ʈ�� ���� ���� ����� 5���� ���� ã��
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.forward, shieldRange, enemyLayer);

        // ���带 �ο��� �ִ� �� ����ŭ�� ����
        int enemiesShielded = 0;
        foreach (RaycastHit hit in hits)
        {
            if (enemiesShielded >= maxEnemiesToShield)
                break;

            GameObject enemy = hit.collider.gameObject;

            // ���� ���带 �̹� ������ �ִ��� Ȯ��
           /* if (!enemy.GetComponent<ShieldEffect>())
            {
                // ������ ���� ȿ�� �߰�
                ShieldEffect shieldEffect = enemy.AddComponent<ShieldEffect>();
                // ���� ���� �ð� ����
                shieldEffect.duration = shieldDuration;

                enemiesShielded++;
            }*/
        }
    }
}
