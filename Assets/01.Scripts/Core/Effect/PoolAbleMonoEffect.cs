using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PoolAbleMonoEffect : PoolableMono
{
    public EffectType EffectType;

    private void Awake()
    {
        RegisterEffect();
    }

    public override void InitializePoolingItem(){}

    protected virtual void RegisterEffect()
    {
        if (EffectType == EffectType.Particle)
            EffectManager.Instance.RegisterEffect(gameObject.GetComponent<ParticleSystem>());
        else if (EffectType == EffectType.Shader)
            EffectManager.Instance.RegisterEffect(gameObject.GetComponent<Material>());
        else if (EffectType == EffectType.VFX)
            EffectManager.Instance.RegisterEffect(gameObject.GetComponent<VisualEffect>());        
    }
}
