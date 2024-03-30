using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Dictionary<string, PoolableMonoEffect> _effects = new Dictionary<string, PoolableMonoEffect>();

    public void RegisterEffect(PoolableMonoEffect effect)
    {
        if (effect == null)
        {
            Debug.LogError($"this object : {effect} is not effect. Check Component Attatched");
            return;
        }

        string effectName = effect.GetType().ToString();
        if (!_effects.ContainsKey(effectName))
        {
            _effects.Add(effectName, effect);
        }
        else
            Debug.Log($"This Object already contain effect. Object : {effect}");
    }

    public PoolableMonoEffect GetEffect(PoolableMonoEffect effect)
    {
        string effectName = effect.GetType().ToString();
        
        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return null;
        }

        return _effects[effectName];
    }

    public PoolableMonoEffect GetEffect(string effectName)
    {
        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return null;
        }

        return _effects[effectName];
    }

    public void PlayEffect(PoolableMonoEffect effect, Vector3 pos)
    {
        string effectName = effect.GetType().ToString();

        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return;
        }

        _effects[effectName].InitializePoolingItem();
        PoolManager.Instance.Pop(effect.poolingType, pos);
    }

    public void StopEffect(PoolableMonoEffect effect)
    {
        string effectName = effect.GetType().ToString();
        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return;
        }

        PoolManager.Instance.Push(_effects[effectName]);
    }

    public void PauseParticle(PoolableMonoEffect effect, float delayTime)
    {
        string effectName = effect.GetType().ToString();
        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return;
        }

        if (_effects[effectName].EffectType == EffectType.Particle)
        {
            Debug.LogError($"{effectName} is not particle.");
            return;
        }

        if (effect.TryGetComponent<ParticleSystem>(out var particle))
        {
            StartCoroutine(ParticlePauseDelay(particle, delayTime));
        }
        else
            Debug.LogError($"{effect} is not particle, Check Component Attatched");
    }

    public void PauseParticle(PoolableMonoEffect effect)
    {
        string effectName = effect.GetType().ToString();
        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return;
        }

        if (_effects[effectName].EffectType == EffectType.Particle)
        {
            Debug.LogError($"{effectName} is not particle.");
            return;
        }

        if (effect.TryGetComponent<ParticleSystem>(out var particle))
        {
            particle.Pause();
        }
        else
            Debug.LogError($"{effect} is not particle, Check Component Attatched");
    }

    public void PauseParticlePlay(PoolableMonoEffect effect)
    {
        string effectName = effect.GetType().ToString();
        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return;
        }

        if (_effects[effectName].EffectType == EffectType.Particle)
        {
            Debug.LogError($"{effectName} is not particle.");
            return;
        }

        if (effect.TryGetComponent<ParticleSystem>(out var particle))
        {
            particle.Play();
        }
        else
            Debug.LogError($"{effect} is not particle, Check Component Attatched");
    }

    private IEnumerator ParticlePauseDelay(ParticleSystem particle, float delayTime)
    {
        particle.Pause();
        yield return new WaitForSeconds(delayTime);
        particle.Play();
    }
}
