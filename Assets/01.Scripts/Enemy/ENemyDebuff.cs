using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENemyDebuff : PoolableMono
{
    public PlayerStat PlayerStat { get; private set; }

    private float poisonDamage = 2f;

    public DebuffType _debuffType;

    public enum DebuffType
    {
        Slow,
        poison,
        push
    }


    public void PoisonPortion()
    {   
        StartCoroutine(PoisonDamage(2));   
    }

    public IEnumerator PoisonDamage(float time)
    {
        while(time < 5)
        {
            PlayerStat.Hit(poisonDamage);
            yield return new WaitForSeconds(time);
        }
    }

    public void SlowPortion()
    {
        float slowPos = 3f;
        PlayerStat.IncreaseStatBy(-slowPos, 3f, PlayerStat.GetStatByType(PlayerStatType.moveSpeed));           
    }

    public void PushPortion()
    {
        float slowPower = 3f;
        GameManager.Instance.player.RigidbodyCompo.AddForce(-GameManager.Instance.player.transform.forward * slowPower , ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_debuffType == DebuffType.poison)
        {
            PoisonPortion();
        }

        else if(_debuffType == DebuffType.push)
        {
            PushPortion();
        }

        else if(_debuffType == DebuffType.Slow)
        {
            SlowPortion();
        }
    }

    public override void InitializePoolingItem()
    {

    }
}