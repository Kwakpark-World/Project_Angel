using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvent : MonoBehaviour
{
    EnemyAI enemyAI;

    public void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    public void OnDie()
    {
        enemyAI.OnDieTrue();
        Destroy(gameObject);
    }
}
