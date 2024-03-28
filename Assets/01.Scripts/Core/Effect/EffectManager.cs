using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.ParticleSystem;

public class EffectManager : MonoBehaviour
{
    private static EffectManager _instance;
    public static EffectManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<EffectManager>();

            if (_instance == null)
                Debug.Log("Effect Manager is null");

            return _instance;
        }
    }

    private HashSet<ParticleSystem> _particles = new HashSet<ParticleSystem>();
    private HashSet<Material> _shaders = new HashSet<Material>();
    private HashSet<VisualEffect> _vfxs = new HashSet<VisualEffect>();

    private Dictionary<string, PoolAbleMonoEffect> _effects = new Dictionary<string, PoolAbleMonoEffect>();

    public void RegisterEffect(PoolAbleMonoEffect effect)
    {
        
    }

    #region RegisterEffect
    public void RegisterEffect(ParticleSystem particle)
    {
        if (particle == null)
        {
            Debug.LogError($"this object : {particle} is not particle. Check Effect Type or Component Attatched");
            return;
        }

        if (_particles.Add(particle))
        {
            if (particle.TryGetComponent<PoolAbleMonoEffect>(out var pool))
            {
                pool.InitializePoolingItem();
            }
            else
                Debug.LogError($"{particle} is not PoolAbleEffect");

        }
        else
            Debug.Log($"This Object already contain particles. Object : {particle}");
        
    }

    public void RegisterEffect(Material shader)
    {
        if (shader == null)
        {
            Debug.LogError($"this object : {shader} is not shader. Check Effect Type or Component Attatched");
            return;
        }

        if (_shaders.Add(shader))
        {

        }
        else
            Debug.Log($"This Object already contain Shaders. Object : {shader}");
       
    }
    
    public void RegisterEffect(VisualEffect vfx)
    {
        if (vfx == null)
        {
            Debug.LogError($"this object : {vfx} is not vfx. Check Effect Type or Component Attatched");
            return;
        }

        if (_vfxs.Add(vfx))
        {
            if (vfx.TryGetComponent<PoolAbleMonoEffect>(out var pool))
            {
                pool.InitializePoolingItem();
            }
            else
                Debug.LogError($"{vfx} is not PoolAbleEffect");
        }
        else
            Debug.Log($"This Object already contain vfxs. Object : {vfx}");
        
    }
    #endregion
}
