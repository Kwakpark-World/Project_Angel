using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlooring : MonoBehaviour
{
   
    public float radius = 5f; // 원의 반지름 설정

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        float angleStep = 10f; 
        Vector3 center = transform.position;

        for (float angle = 0; angle < 360; angle += angleStep)
        {
            float startX = center.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float startZ = center.z + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            float endX = center.x + radius * Mathf.Cos(Mathf.Deg2Rad * (angle + angleStep));
            float endZ = center.z + radius * Mathf.Sin(Mathf.Deg2Rad * (angle + angleStep));

            Vector3 startPoint = new Vector3(startX, center.y, startZ);
            Vector3 endPoint = new Vector3(endX, center.y, endZ);
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.collider.CompareTag("Player"))
        { 
            //여기서 플레이어 데미지 입히게 하긴 
        }
    }
}
