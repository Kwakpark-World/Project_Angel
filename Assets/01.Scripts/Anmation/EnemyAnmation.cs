using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnmation : MonoBehaviour
{
    public ParticleSystem knghtAttackParticle;

    public UnityEvent animationEndEvent;

    public void OnAttack()
    {
        knghtAttackParticle.Play();
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
