using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : PoolableMonoEffect
{
    private Transform bloodTransform;

    public void ViewPlayerBlood()
    {
        //일단 맞은 적에게 해야하고 5초뒤에 없어져야하니깐 그걸 여기서 해야하나?
        //Vector3 playerPosition = GameManager.Instance.PlayerInstance.transform.position;

        //Vector3 direction = playerPosition - bloodTransform.position;
        //direction.Normalize();

        //float moveSpeed = 1.0f;
        //transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
