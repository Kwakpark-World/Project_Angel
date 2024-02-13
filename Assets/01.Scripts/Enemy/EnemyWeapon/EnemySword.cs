using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public float meleeAttackDamage = 10f;
    private bool canDamage = true;
    private float resetTime = 1.1f;

    public Player player;
    public EnemyAI enemyAI;
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
                    player.TakeDamage(meleeAttackDamage);
                }

                StartCoroutine(ResetDamageTimer());
            }
        }
    }

    private IEnumerator ResetDamageTimer()
    {
        canDamage = false;

        yield return new WaitForSeconds(resetTime);

        canDamage = true;
    }
}
