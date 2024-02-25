using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goat : MonoBehaviour
{
    [SerializeField]
    private float _chaseDuration;
    [SerializeField]
    private float _chaseSpeed;
    private Player player;
    private float _chaseTimer;

    private void OnEnable()
    {
        player = FindAnyObjectByType<Player>();
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
            // Hit player.
        }

        // Push this object to pool.
    }
}
