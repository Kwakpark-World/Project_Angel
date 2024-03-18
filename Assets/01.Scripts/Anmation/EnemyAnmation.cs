using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnmation : MonoBehaviour
{
    public EnemyType enemyType;
    public EnemyAI enemyAi;
    public ParticleSystem knghtAttackParticle;

    public GameObject WeaponSpawn;

    public UnityEvent animationEndEvent;

    public void OnAttack()
    {
        if(enemyType == EnemyType.knight)
        {
            knghtAttackParticle.Play();
            
        }

        if (enemyType == EnemyType.witcher)
        {
            PoolableMono EnemyArrow = PoolManager.instance.Pop(PoolingType.Porison);
            EnemyArrow.transform.position = WeaponSpawn.transform.position;
            EnemyArrow.transform.rotation = WeaponSpawn.transform.rotation;
            Debug.Log("1");
        }
        
    }

    public void OnKnightSound()
    {
        SoundManager.Instance.PlayAttackSound("Attack1");
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
