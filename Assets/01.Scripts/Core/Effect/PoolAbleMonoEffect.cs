using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class PoolableMonoEffect : PoolableMono
{
    public EffectType EffectType;

    private void Awake()
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
            if (this.TryGetComponent<ParticleSystem>(out ParticleSystem particle))
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
