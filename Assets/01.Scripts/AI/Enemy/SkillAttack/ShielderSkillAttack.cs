using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderSkillAttack : EnemyAttack
{

    private Transform m_transform;
    public LayerMask enemyLayer;
    private int maxEnemiesToShield = 5; 

    // Start is called before the first frame update
    void Update()
    {
        ApplyShield();
    }

    void ApplyShield()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.forward, 5, enemyLayer);

        int enemiesShielded = 0;
        foreach (RaycastHit hit in hits)
        {
            if (enemiesShielded >= maxEnemiesToShield)
                break;

            GameObject enemy = hit.collider.gameObject;
            Debug.Log(enemy);

            //shieldEffect.duration = shieldDuration;
        }
    }

    // Fill down.

    public override void OnStart()
    {

    }

    public override void OnStop()
    {

    }

    public override Node.State OnUpdate()
    {
        return Node.State.Success;
    }
}
