using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimation : MonoBehaviour
{
    public EnemyType enemyType;
    public EnemyBrain enemyBrain;

    public ParticleSystem knghtAttackParticle;

    public UnityEvent animationEndEvent;

    public void OnAttack()
    {

        if(enemyType == EnemyType.Knight)
        {
            knghtAttackParticle.Play();
            SoundManager.Instance.PlayAttackSound("Attack1");
        }

        if(enemyType == EnemyType.Archer)
        {
            EnemyArrow EnemyArrow = PoolManager.Instance.Pop(PoolingType.Arrow) as EnemyArrow;
            EnemyArrow.owner = enemyBrain;
            //EnemyArrow.transform.position = enemyBrain.WeaponSpawn.transform.position;
            //EnemyArrow.transform.rotation = enemyBrain.WeaponSpawn.transform.rotation;
            SoundManager.Instance.PlayAttackSound("Attack2");
        }

        if (enemyType == EnemyType.Witch)
        {
            
            DebuffPotion EnemyDebuff = PoolManager.Instance.Pop(PoolingType.PoisonPotion) as DebuffPotion;
            EnemyDebuff.owner = enemyBrain;
            //EnemyDebuff.transform.position = enemyBrain.WeaponSpawn.transform.position;
            //EnemyDebuff.transform.rotation = enemyBrain.WeaponSpawn.transform.rotation;
            
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
