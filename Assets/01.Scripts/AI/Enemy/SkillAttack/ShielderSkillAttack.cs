using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ShielderSkillAttack : MonoBehaviour
{
    public float shieldDuration = 3f; 
    public LayerMask enemyLayer;
    public int maxEnemiesToShield = 5; 
    public float shieldRange = 10f;

    public GameObject shieldEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ApplyShieldToEnemies());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(ApplyShieldToEnemies());
        }
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

            GameObject shieldEffect = Instantiate(shieldEffectPrefab, enemy.transform.position, Quaternion.identity);
            Destroy(shieldEffect, shieldDuration);

            //shieldEffect.duration = shieldDuration;
        }
    }
}
