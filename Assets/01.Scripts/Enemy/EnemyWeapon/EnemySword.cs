using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    private bool canDamage = true;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyBrain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
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
