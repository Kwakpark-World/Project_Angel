using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : PoolableMonoEffect
{
    private Transform bloodTransform;

    public void ViewPlayerBlood()
    {
        Vector3 playerPosition = GameManager.Instance.playerTransform.position;

        Vector3 direction = playerPosition - bloodTransform.position;
        direction.Normalize();

        float moveSpeed = 1.0f; 
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
