using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RuneManager : MonoSingleton<RuneManager>
{
    public Dictionary<RuneType, List<Rune>> _collectedRunes;
    [SerializeField] private RuneListSO _runeList;

    public float _runSynergy;

    public bool _islastDance = false;

    public bool isArmor = false;
    public bool isDebuff = false;


    private void Awake()
    {
        _collectedRunes = new Dictionary<RuneType, List<Rune>>();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            CreateRune();
        }

        //Debug.Log(_collectedRunes);

        ActivateRune();
    }

    public Rune CreateRune()
    {
        Rune rune = PoolManager.Instance.Pop(PoolingType.Rune) as Rune;
        RuneEffectSO runeData = _runeList[Random.Range(0, _runeList.list.Count)];
        rune.SetRuneData(runeData);
        _runeList.Remove(runeData);

        RegisterRune(rune);

        rune.SetDissolve(0.55f);
        return rune;
    }

    public void RegisterRune(Rune rune)
    {
        if(_collectedRunes.TryGetValue(rune.RuneData.runeType, out List<Rune> runes))
        {
            runes.Add(rune);
        }
        else
        {
            List<Rune> list = new List<Rune> { rune };
            _collectedRunes.Add(rune.RuneData.runeType, list);
        }
    }

    public void SetRuneList(RuneListSO list)
    {
        _runeList = list;
    }

    public void ActivateRune()
    {
        
        foreach (var kvp in _collectedRunes)
        {
            RuneType runeType = kvp.Key;
            List<Rune> runes = kvp.Value;

            if (runeType == RuneType.STRENGTH && runes.Count >= 3)
            {
                Debug.Log($"{runeType}");
            }

            if(runeType == RuneType.Acceleration && runes.Count >= 3)
            {
                //준호가 공격 만들면 하기 거기서 그냥 2타 했을 때 일시적으로 속도 빨라주게만 하면 됨
                StartCoroutine(RuneCooldownCoroutine(30));
                Debug.Log($"{runeType}");
            }

            if(runeType == RuneType.HEALTH && runes.Count >= 3)
            {
                StartCoroutine(lastDance(5f));
                
                StartCoroutine(RuneCooldownCoroutine(300));
                Debug.Log($"{runeType}");
            }

            if(runeType == RuneType.DEBUFF && runes.Count >= 3)
            {
                Debug.Log($"{runeType}");
                //여기서 때릴때마다 확인 시키기
                int Randomvalue = Random.Range(0, 1);

                if(Randomvalue == 1)
                {
                    isDebuff = true;
                }
            }

            if(runeType == RuneType.ARMOR && runes.Count >= 3)
            {
                isArmor = false;
                StartCoroutine(RuneCooldownCoroutine(60));
                Debug.Log($"{runeType}");
            }
        }
    }

    IEnumerator RuneCooldownCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var kvp in _collectedRunes)
        {
            RuneType runeType = kvp.Key;
            List<Rune> runes = kvp.Value;

            if (runeType == RuneType.STRENGTH && runes.Count >= 3)
            {
                // STRENGTH rune logic
                Debug.Log($"You have collected 3 or more runes of type {runeType}.");
            }

            if (runeType == RuneType.Acceleration && runes.Count >= 3)
            {
                // Acceleration rune logic
            }

            if (runeType == RuneType.HEALTH && runes.Count >= 3)
            {
                // HEALTH rune logic
            }

            if (runeType == RuneType.DEBUFF && runes.Count >= 3)
            {
                
                isDebuff = true;
            }

            if (runeType == RuneType.ARMOR && runes.Count >= 3)
            {
                
                isArmor = true;
            }
        }
    }

    IEnumerator lastDance(float time)
    {
        yield return new WaitForSeconds(5f);
         
        if (GameManager.Instance.PlayerInstance.CurrentHealth <= 1 && _islastDance == false)
        {
            _islastDance = true;
        }
        _islastDance = false;
    }
}
