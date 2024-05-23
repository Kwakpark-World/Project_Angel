using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableMonoEffect : PoolableMono
{
    [HideInInspector]
    public ParticleSystem[] particles;
    protected float duration = 0f;

    protected virtual void Awake()
    {
        RegisterEffect();
    }

    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// Setting parentTrm, rotation, scale, etc.. , 
    /// If the type is particle, it basically starts here.
    /// </summary>
    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        foreach (ParticleSystem particle in particles)
        {
            duration = Mathf.Max(particle.main.duration, duration);

            particle.Play();
        }
    }

    public virtual void RegisterEffect()
    {
        EffectManager.Instance.RegisterEffect(poolType);

        particles = GetComponentsInChildren<ParticleSystem>();
    }
}
