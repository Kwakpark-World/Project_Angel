using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargingAuraEffect : PlayerEffect
{
    public float moveSpeed = 1;
    public float damage = 3;

    private Vector3 auraRot = new Vector3(-85f, -90f, 0f);
    private Vector3 auraOnceMoreRot = new Vector3(-98f, -90f, 0f);

    private Rigidbody rb;

    private HashSet<Brain> enemyNormalHitDuplicateChecker = new HashSet<Brain>();
    
    public override void InitializePoolItem()
    {
        base.InitializePoolItem();
        rb = GetComponent<Rigidbody>();

        enemyNormalHitDuplicateChecker.Clear();

        Vector3 playerRot = _player.transform.eulerAngles;
        playerRot.x = 0f;
        playerRot.z = 0f;

        if (_player.isOnChargingSlashOnceMore)
            transform.rotation = Quaternion.Euler(playerRot + auraOnceMoreRot);
        else
            transform.rotation = Quaternion.Euler(playerRot + auraRot);

        PoolManager.Instance.Push(this, 2f);
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        rb.velocity = _player.transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Brain>(out Brain enemy))
        {
            if (enemyNormalHitDuplicateChecker.Add(enemy)){
                enemy.OnHit(damage);

                _player.CurrentAwakenGauge++;
            }
        }
    }
}
