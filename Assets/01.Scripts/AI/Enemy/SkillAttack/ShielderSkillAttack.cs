using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderSkillAttack : MonoBehaviour
{
    public float shieldDuration = 3f; 
    public LayerMask enemyLayer;
    public int maxEnemiesToShield = 5; 
    public float shieldRange = 10f; 

    // Start is called before the first frame update
    void Start()
    {
        // 3초마다 지속되는 쉴드를 적에게 적용
        StartCoroutine(ApplyShieldToEnemies());
    }

    IEnumerator ApplyShieldToEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(shieldDuration);
            ApplyShield();
        }
    }

    void ApplyShield()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.forward, shieldRange, enemyLayer);

        int enemiesShielded = 0;
        foreach (RaycastHit hit in hits)
        {
            if (enemiesShielded >= maxEnemiesToShield)
                break;

            GameObject enemy = hit.collider.gameObject;

            //shieldEffect.duration = shieldDuration;
        }
    }
}
