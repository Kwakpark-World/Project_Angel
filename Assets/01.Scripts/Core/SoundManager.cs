using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum SoundMode
{
    None,
    Intro,
    NonCombat,
    Combat
}

public enum EnvSoundType
{
    Wind,
}

public enum SoundEffectType
{
    Button,
    Scroll,
    Buy,
}

[Serializable]
public struct SoundModeSource
{
    public SoundMode mode;
    public AudioSource source;
}

[Serializable]
public struct EnvSoundSource
{
    public EnvSoundType type;
    public AudioSource source;
}

[Serializable]
public struct SoundEffectClip
{
    public SoundEffectType type;
    public AudioClip clip;
}


public class SoundManager : MonoSingleton<SoundManager>
{
    public SoundModeSource[] bgmList;
    public EnvSoundSource[] envList;
    public SoundEffectClip[] sfxList;
    public AudioMixerGroup sfxGroup;
    private Dictionary<SoundMode, AudioSource> _bgmDictionary = new Dictionary<SoundMode, AudioSource>();
    private Dictionary<EnvSoundType, AudioSource> _envDictionary = new Dictionary<EnvSoundType, AudioSource>();
    private Dictionary<SoundEffectType, AudioClip> _sfxDicionary = new Dictionary<SoundEffectType, AudioClip>();    private SoundMode _soundMode = SoundMode.None;

    private void Awake()
    {
        foreach (SoundModeSource bgm in bgmList)
        {
            _bgmDictionary.Add(bgm.mode, bgm.source);
        }

        foreach (EnvSoundSource env in envList)
        {
            _envDictionary.Add(env.type, env.source);
        }

        foreach (SoundEffectClip sfx in sfxList)
        {
            _sfxDicionary.Add(sfx.type, sfx.clip);
        }

        

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeBGMMode(SoundMode mode)
    {
        if (_soundMode != SoundMode.None)
        {
            _bgmDictionary[_soundMode].Stop();
        }

        _soundMode = mode;

        _bgmDictionary[_soundMode].Play();
    }

    public void PlayEnv(EnvSoundType type)
    {
        _envDictionary[type].Play();
    }

    public void StopEnv(EnvSoundType type)
    {
        _envDictionary[type].Stop();
    }

    public void PlaySFX(SoundEffectType type)
    {
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = transform.position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = _sfxDicionary[type];
        audioSource.outputAudioMixerGroup = sfxGroup;
        audioSource.Play();
        Destroy(gameObject, _sfxDicionary[type].length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }
}