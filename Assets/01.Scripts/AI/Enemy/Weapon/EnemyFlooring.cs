using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlooring : MonoBehaviour
{

    public float radius = 5f; // 원의 반지름 설정
    private bool shouldDrawGizmos = false;
    public Vector3 collisionPoint;

    public EnemyAnimator enemyAnimator;

/*    private void Update()
    {
        shouldDrawGizmos = true;
    }*/

    private void OnDrawGizmos()
    {
        //if (!shouldDrawGizmos) return;

        Gizmos.color = Color.red;
        float angleStep = 10f;
        Vector3 center = collisionPoint;

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

        if (GameManager.Instance.PlayerInstance.transform != null)
        {
            Vector3 playerPosition = GameManager.Instance.PlayerInstance.transform.position;
            float distance = Vector3.Distance(center, playerPosition);

            if (distance <= radius)
            {
                Debug.Log("플레이어가 원 안에 있습니다.");
            }
            else
            {
                Debug.Log("플레이어가 원 밖에 있습니다.");
            }
        }
    }
}
