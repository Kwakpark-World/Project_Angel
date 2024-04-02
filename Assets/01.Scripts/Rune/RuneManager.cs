using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class RuneManager : MonoBehaviour
{
    private static RuneManager _instance;
    public static RuneManager Instance 
    { 
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<RuneManager>();
                if(_instance == null)
                {
                    Debug.LogError("룬매니저 넣어라 좀");
                }
            }
            return _instance;
        }
    }

    private Dictionary<RuneType, List<Rune>> _collectedRunes;
    [SerializeField] private RuneListSO _runeList;

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
        foreach (var synergyGroup in _collectedRunes.GroupBy(kvp => kvp.Value.FirstOrDefault()?.RuneData.synergyGroup))
        {
            if (synergyGroup.Count() >= 3)
            {
                Debug.Log($"시너지 그룹 {synergyGroup.Key}에 속하는 룬이 3개 이상 있습니다.");
            }
        }
    }

}
