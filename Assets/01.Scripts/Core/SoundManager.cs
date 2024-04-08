using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public AudioSource bgmSound;
    public AudioClip[] bgmList;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneload;
        }

        else if(instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnSceneload(Scene arg0, LoadSceneMode arg1)
    {
        for(int i =0; i < bgmList.Length; i++)
        {
            if(arg0.name == bgmList[i].name)
            {
                BGMPlay(bgmList[i]);
            }
        }
    }

    private void BGMPlay(AudioClip audioClip)
    {
        bgmSound.clip = audioClip;
        bgmSound.loop = true;
        bgmSound.volume = 1.0f;
        bgmSound.Play();
    }
}