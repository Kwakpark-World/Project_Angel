using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : PoolableMonoEffect
{
    private Transform bloodTransform;

    public void ViewPlayerBlood()
    {
        //�ϴ� ���� ������ �ؾ��ϰ� 5�ʵڿ� ���������ϴϱ� �װ� ���⼭ �ؾ��ϳ�?
        //Vector3 playerPosition = GameManager.Instance.PlayerInstance.transform.position;

        //Vector3 direction = playerPosition - bloodTransform.position;
        //direction.Normalize();

        //float moveSpeed = 1.0f;
        //transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
