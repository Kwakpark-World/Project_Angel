using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    EnemyAI enemyAI;
    EnemyStats enemyStats;

    public float damage;

    private void Awake()
    {
        enemyAI = FindObjectOfType<EnemyAI>();
        enemyStats = FindObjectOfType<EnemyStats>();
    }

    private void Start()
    {
        

        damage = enemyStats._damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemyAI != null)
        {
            enemyAI.OnDamage(damage);
            Debug.Log("¾Æ¾ß");
        }
    }
}
