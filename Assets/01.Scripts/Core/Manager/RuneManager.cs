using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuneManager : MonoSingleton<RuneManager>
{
    [SerializeField]
    private RuneListSO _runeList;

    private List<RuneDataSO> _equipedRunes = Enumerable.Repeat<RuneDataSO>(null, 5).ToList();
    private BuffType _synergizeRuneType;

    protected override void Awake()
    {
        base.Awake();

        _runeList = Instantiate(_runeList);

        for (int i = 0; i < _equipedRunes.Count; ++i)
        {
            _equipedRunes[i] = null;
        }
    }

    public void Update()
    {
#if UNITY_EDITOR // Debug
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            SpawnRune(GameManager.Instance.PlayerInstance.transform.position + Vector3.forward * 5f);
        }
#endif
    }

    public void SetRuneList(RuneListSO list)
    {
        _runeList = list;
    }

    public void SpawnRune(Vector3 runeSpawnPos)
    {
        Rune rune = PoolManager.Instance.Pop(PoolingType.Rune, runeSpawnPos) as Rune;
        RuneDataSO runeData = _runeList.list[UnityEngine.Random.Range(0, _runeList.list.Count)];

        rune.InitializeRune(runeData);
        _runeList.list.Remove(runeData);
    }

    public bool TryEquipRune(RuneDataSO runeData)
    {
        if (!runeData)
        {
            return false;
        }

        for (int i = 0; i < _equipedRunes.Count; ++i)
        {
            if (!_equipedRunes[i])
            {
                _equipedRunes[i] = runeData;

                break;
            }

            if (i == 4)
            {
                return false;
            }
        }

        CheckRuneSynergy(runeData.synergyType);

        return true;
    }

    public bool UnequipRune(int index)
    {
        if (!_equipedRunes[index])
        {
            return false;
        }

        _equipedRunes[index] = null;

        return true;
    }

    public RuneDataSO GetEquipedRune(int index)
    {
        return _equipedRunes[index];
    }

    public void CheckRuneSynergy(BuffType runeSynergyType)
    {
        if (_equipedRunes.FindAll((runeData) => runeData && (runeData.synergyType == runeSynergyType)).Count >= 3)
        {
            GameManager.Instance.PlayerInstance.BuffCompo.StopBuff(_synergizeRuneType);
            GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(runeSynergyType);

            _synergizeRuneType = runeSynergyType;
        }
        else if (_equipedRunes.FindAll((runeData) => runeData && (runeData.synergyType == _synergizeRuneType)).Count < 3)
        {
            GameManager.Instance.PlayerInstance.BuffCompo.StopBuff(_synergizeRuneType);

            _synergizeRuneType = BuffType.None;
        }
    }
}
