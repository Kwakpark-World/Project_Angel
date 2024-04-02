using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RuneManager : MonoSingleton<RuneManager>
{
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
        /*foreach (var synergyGroup in _collectedRunes.GroupBy(kvp => kvp.Value.FirstOrDefault()?.RuneData.)
        {
            if (synergyGroup.Count() >= 3)
            {
                Debug.Log($"�ó��� �׷� {synergyGroup.Key}�� ���ϴ� ���� 3�� �̻� �ֽ��ϴ�.");
            }
        }*/
    }

}
