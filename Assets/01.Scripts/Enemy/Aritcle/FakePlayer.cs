using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    private void Start()
    {
        // ���÷� FakePlayer�� ������ �� ���� �������� �޵��� ��
        EnemyAI enemyAI = FindObjectOfType<EnemyAI>(); // ���� AI ��ũ��Ʈ�� ã�ƿ�
    }

    public void TakeDamage(float damage)
    {
        // ���⿡ �������� �޴� ���� �߰�
        // �� ���ÿ����� �ܼ��� �������� ���
        Debug.Log("FakePlayer�� " + damage + "�� �������� �޾ҽ��ϴ�.");

        // ���⿡�� �ʿ��� �߰����� ������ ������ �� �ֽ��ϴ�.
    }
}
