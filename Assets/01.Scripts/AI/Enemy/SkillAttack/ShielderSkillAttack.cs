using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ShielderSkillAttack : MonoBehaviour
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
}
