using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuffArea : PoolableMono
{
    [HideInInspector]
    public EnemyBrain owner;
    [SerializeField]
    private BuffType _areaBuffType;
    [SerializeField]
    private float _radius = 5f; // 원의 반지름 설정
    [SerializeField]
    private float _height = 0.5f;
    [SerializeField]
    private bool shouldDrawGizmos = false;

    private void Update()
    {
        Vector3 center = transform.position;

        if (GameManager.Instance.HasPlayer)
        {
            Player player = GameManager.Instance.PlayerInstance;
            Vector3 direction = player.playerCenter.position - center;

            float distanceXZ = new Vector3(direction.x, 0f, direction.z).sqrMagnitude;
            float distanceY = direction.y;

            if (distanceXZ <= _radius * _radius)
            {
                if (distanceY <= _height * 0.5f)
                {
                    switch (_areaBuffType)
                    {
                        case BuffType.Potion_Poison:
                            GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_areaBuffType, owner.BuffCompo.BuffStatData.poisonDuration, owner);

                            break;

                        case BuffType.Potion_Freeze:
                            GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_areaBuffType, owner.BuffCompo.BuffStatData.freezeDuration, owner);

                            break;

                        case BuffType.Potion_Paralysis:
                            GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_areaBuffType, owner.BuffCompo.BuffStatData.paralysisDuration, owner);

                            break;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!shouldDrawGizmos) return;

        Gizmos.color = Color.red;
        float angleStep = 10f;
        Vector3 center = transform.position;

        for (float angle = 0; angle < 360; angle += angleStep)
        {
            float startX = center.x + _radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float startZ = center.z + _radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            float endX = center.x + _radius * Mathf.Cos(Mathf.Deg2Rad * (angle + angleStep));
            float endZ = center.z + _radius * Mathf.Sin(Mathf.Deg2Rad * (angle + angleStep));

            Vector3 startPoint = new Vector3(startX, center.y, startZ);
            Vector3 endPoint = new Vector3(endX, center.y, endZ);
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }

    public override void InitializePoolItem()
    {
        PoolManager.Instance.Push(this, 2f);
    }
}
