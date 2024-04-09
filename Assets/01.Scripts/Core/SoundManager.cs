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
public struct SoundEffectClip
{
    public SoundEffectType mode;
    public AudioClip clip;
}

public class SoundManager : MonoSingleton<SoundManager>
{
    public SoundModeSource[] bgmList;
    public SoundEffectClip[] sfxList;
    public AudioMixerGroup sfxGroup;
    private Dictionary<SoundMode, AudioSource> _bgmDictionary = new Dictionary<SoundMode, AudioSource>();
    private Dictionary<SoundEffectType, AudioClip> _sfxDicionary = new Dictionary<SoundEffectType, AudioClip>();
    private SoundMode _soundMode = SoundMode.None;

    private void Awake()
    {
        foreach (SoundModeSource bgm in bgmList)
        {
            _bgmDictionary.Add(bgm.mode, bgm.source);
        }

        foreach (SoundEffectClip bgm in sfxList)
        {
            _sfxDicionary.Add(bgm.mode, bgm.clip);
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

    public void PlaySFX(SoundEffectType mode)
    {
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = transform.position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = _sfxDicionary[mode];
        audioSource.outputAudioMixerGroup = sfxGroup;
        audioSource.Play();
        Destroy(gameObject, _sfxDicionary[mode].length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }
}