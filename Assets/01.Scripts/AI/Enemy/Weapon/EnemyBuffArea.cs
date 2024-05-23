using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuffArea : MonoBehaviour
{
    [SerializeField]
    private BuffType _buffType;
    [SerializeField]
    private EnemyBrain _owner;
    public float radius = 5f; // 원의 반지름 설정
    private bool shouldDrawGizmos = false;
    public Vector3 collisionPoint;

    //public EnemyAnimator enemyAnimator;

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
            Player player = GameManager.Instance.PlayerInstance;
            float distance = Vector3.Distance(center, player.transform.position);

            if (distance <= radius)
            {
                
                 player.BuffCompo.PlayBuff(_buffType,_owner.BuffCompo.BuffStatData.poisonDuration, _owner);
                
            }
            else
            {
                Debug.Log("플레이어가 원 밖에 있습니다.");
            }
        }
    }
}
