using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [HideInInspector]
    public Collider swordCollider;
    [SerializeField]
    private EnemyBrain _owner;

    private void Awake()
    {
        swordCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.PlayerInstance.gameObject)
        {
            GameManager.Instance.PlayerInstance.OnHit(_owner.EnemyStatData.GetAttackPower());

            swordCollider.enabled = false;
        }
    }
}
