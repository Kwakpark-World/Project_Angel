using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    private bool canDamage = true;

    public Player player;
    EnemyAI enemyAI;

    private void Awake()
    {
        player.GetComponent<Player>();
        enemyAI = GetComponent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (canDamage)
            {
                if (player != null)
                {
                   player.PlayerStat.Hit(enemyAI.EnemyStatistic.GetAttackPower());
                }

                StartCoroutine(ResetDamageTimer());
            }
        }
    }

    private IEnumerator ResetDamageTimer()
    {
        canDamage = false;

        yield return new WaitForSeconds(enemyAI.EnemyStatistic.GetAttackDelay());

        canDamage = true;
    }
}
