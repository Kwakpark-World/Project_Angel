using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goat : MonoBehaviour
{
    [HideInInspector]
    public Player player = null;

    #region Debug
    [Header("Debug statistics")]
    [SerializeField]
    private float _chaseSpeed;
    [SerializeField]
    private float _chaseDuration;
    private float _chaseTimer;

    [SerializeField]
    private float _attackPower;
    #endregion

    private void OnEnable()
    {
        _chaseTimer = Time.time;
    }

    private void Update()
    {
        if (Time.time > _chaseTimer + _chaseDuration)
        {
            // Push this object to pool.
        }

        if (player != null)
        {
            // Chase player.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            player.TakeDamage(_attackPower);
        }

        // Push this object to pool.
    }
}
