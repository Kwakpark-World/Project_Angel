using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(SoundManager).Name;
                    instance = obj.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }

    private Dictionary<string, AudioSource> attackSoundsDictionary = new Dictionary<string, AudioSource>();

    public AudioSource[] attackSounds;

    private void Start()
    {
        for (int i = 0; i < attackSounds.Length; i++)
        {
            string identifier = "Attack" + (i + 1);
            AddAttackSound(identifier, attackSounds[i]);
        }
    }

    public void AddAttackSound(string identifier, AudioSource sound)
    {
        if (!attackSoundsDictionary.ContainsKey(identifier))
        {
            attackSoundsDictionary.Add(identifier, sound);
        }
    }

    public void PlayAttackSound(string identifier)
    {
        if (attackSoundsDictionary.ContainsKey(identifier))
        {
            attackSoundsDictionary[identifier].Play();
        }
    }

    public void StopAttackSound(string identifier)
    {
        if (attackSoundsDictionary.ContainsKey(identifier))
        {
            attackSoundsDictionary[identifier].Stop();
        }
    }
}
