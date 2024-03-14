using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvent : MonoBehaviour
{
    EnemyBrain enemyAI;

    public void Awake()
    {
        enemyAI = GetComponent<EnemyBrain>();
    }

    public void OnDie()
    {
        enemyAI.OnDieTrue();
        Destroy(gameObject);
    }
}
