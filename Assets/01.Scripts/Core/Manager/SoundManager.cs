using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum BGMMode
{
    None,
    Intro,
    NonCombat,
    Combat
}

public enum ENVType
{
    Wind,
}

public enum SFXType
{
    Enemy_Attack_Knight,
    Enemy_Attack_Ranger,
    Enemy_Attack_Ranger_Reload,
    Enemy_Attack_Witch,
    Enemy_Hit,
    Enemy_Die,
    UI_Button_Hover,
    UI_Button_Click,
}

[Serializable]
public struct BGMModeSource
{
    public BGMMode mode;
    public AudioSource source;
}

[Serializable]
public struct ENVTypeSource
{
    public ENVType type;
    public AudioSource source;
}

[Serializable]
public struct SFXTypeClip
{
    public SFXType type;
    public AudioClip clip;
}

public class SoundManager : MonoSingleton<SoundManager>
{
    public BGMModeSource[] bgmList;
    public ENVTypeSource[] envList;
    public SFXTypeClip[] sfxList;
    public AudioMixer audioMixer;
    private Dictionary<BGMMode, AudioSource> _bgmDictionary = new Dictionary<BGMMode, AudioSource>();
    private Dictionary<ENVType, AudioSource> _envDictionary = new Dictionary<ENVType, AudioSource>();
    private Dictionary<SFXType, AudioClip> _sfxDicionary = new Dictionary<SFXType, AudioClip>();
    private BGMMode _soundMode = BGMMode.None;

    protected override void Awake()
    {
        foreach (BGMModeSource bgm in bgmList)
        {
            _bgmDictionary.Add(bgm.mode, bgm.source);
        }

        foreach (ENVTypeSource env in envList)
        {
            _envDictionary.Add(env.type, env.source);
        }

        foreach (SFXTypeClip sfx in sfxList)
        {
            _sfxDicionary.Add(sfx.type, sfx.clip);
        }
    }

    public void ChangeBGMMode(BGMMode mode)
    {
        if (!_bgmDictionary.ContainsKey(mode))
        {
            Debug.LogError($"{mode} mode BGM source has not exist.");

            return;
        }

        if (_soundMode != BGMMode.None)
        {
            _bgmDictionary[_soundMode].Stop();
        }

        _soundMode = mode;

        _bgmDictionary[_soundMode].Play();
    }

    public void PlayEnv(ENVType type)
    {
        if (!_envDictionary.ContainsKey(type))
        {
            Debug.LogError($"{type} type ENV source has not exist.");

            return;
        }

        _envDictionary[type].Play();
    }

    public void StopEnv(ENVType type)
    {
        if (!_envDictionary.ContainsKey(type))
        {
            Debug.LogError($"{type} type ENV source has not exist.");

            return;
        }

        _envDictionary[type].Stop();
    }

    public void PlaySFX(SFXType type)
    {
        if (!_sfxDicionary.ContainsKey(type))
        {
            Debug.LogError($"{type} type SFX clip has not exist.");

            return;
        }

        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = transform.position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = _sfxDicionary[type];
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        audioSource.Play();
        Destroy(gameObject, _sfxDicionary[type].length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
        // Change to pool.
    }
}