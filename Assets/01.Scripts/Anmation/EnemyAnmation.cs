using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnmation : MonoBehaviour
{
    public EnemyType enemyType;
    public EnemyAI enemyAi;

    public ParticleSystem knghtAttackParticle;

    public UnityEvent animationEndEvent;

    public void OnAttack()
    {

        if(enemyType == EnemyType.knight)
        {
            knghtAttackParticle.Play();
            SoundManager.Instance.PlayAttackSound("Attack1");
        }

        if(enemyType == EnemyType.archer)
        {
            EnemyArrow EnemyArrow = PoolManager.instance.Pop(PoolingType.Arrow) as EnemyArrow;
            EnemyArrow.enemyAI = enemyAi;
            EnemyArrow.transform.position = enemyAi.WeaponSpawn.transform.position;
            EnemyArrow.transform.rotation = enemyAi.WeaponSpawn.transform.rotation;
            SoundManager.Instance.PlayAttackSound("Attack2");
        }

        if (enemyType == EnemyType.witcher)
        {
            
            ENemyDebuff EnemyDebuff = PoolManager.instance.Pop(PoolingType.poison) as ENemyDebuff;
            EnemyDebuff.enemyAI = enemyAi;
            EnemyDebuff.transform.position = enemyAi.WeaponSpawn.transform.position;
            EnemyDebuff.transform.rotation = enemyAi.WeaponSpawn.transform.rotation;
            
        }
        
    }

    public void OnArcherSound()
    {
        SoundManager.Instance.PlayAttackSound("Attack3");
    }

    public void OnAnimationEnd()
    {
        animationEndEvent?.Invoke();
    }
}
