using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    private bool canDamage = true;

    [SerializeField]EnemyAI enemyAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (canDamage)
            {
                if (GameManager.Instance.player != null)
                {
                   GameManager.Instance.player.PlayerStat.Hit(enemyAI.EnemyStatistic.GetAttackPower());
                    Debug.Log("3");
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
