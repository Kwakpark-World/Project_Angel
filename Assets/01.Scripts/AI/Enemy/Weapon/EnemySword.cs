using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [SerializeField]
    private EnemyBrain _owner;
    public Collider swordCollider;

    private void Awake()
    {
        swordCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            GameManager.Instance.player.PlayerStatData.Hit(_owner.EnemyStatData.GetAttackPower());
        }
    }
}
