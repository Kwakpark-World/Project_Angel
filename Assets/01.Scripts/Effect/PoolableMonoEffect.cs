using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableMonoEffect : PoolableMono
{
    public EffectType EffectType;


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
    public override void InitializePoolingItem()
    {
        if (EffectType == EffectType.Particle)
        {
            ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }
        }
    }

    public virtual void RegisterEffect()
    {
        EffectManager.Instance.RegisterEffect(poolingType);  
    }

}