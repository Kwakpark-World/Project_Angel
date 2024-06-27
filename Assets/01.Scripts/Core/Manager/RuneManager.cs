using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuneManager : MonoSingleton<RuneManager>
{
    private readonly string _assetPath = "Assets/11.SO/Rune";

    [SerializeField]
    private RuneListSO _runeList;
    [field: SerializeField]
    public float UnequipWaitTime { get; private set; }
    public float UnequipWaitTimer { get; set; }
    [field: SerializeField]
    public int EquipableRuneAmount { get; private set; }

    private List<RuneDataSO> _equipedRunes = Enumerable.Repeat<RuneDataSO>(null, 5).ToList();
    private BuffType _synergizeRuneType;

    private List<Rune> _runes;

    protected override void Awake()
    {
        base.Awake();

        _runeList = Instantiate(_runeList);

        for (int i = 0; i < _equipedRunes.Count; ++i)
        {
            _equipedRunes[i] = null;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeRunes();
        _runeList.IsDestroyed();
    }

    public void SetRuneList(RuneListSO list)
    {
        _runeList = list;
    }

    public void SpawnRune(Vector3 runeSpawnPos)
    {
        RuneDataSO runeData;

        if (_runeList.list.Count <= 0)
        {
            string[] assetNames = AssetDatabase.FindAssets("", new[] { _assetPath });

            foreach (string assetName in assetNames)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetName);
                runeData = AssetDatabase.LoadAssetAtPath<RuneDataSO>(path);

                if (!runeData)
                {
                    continue;
                }

                _runeList.list.Add(runeData);
            }
        }

        Rune rune = PoolManager.Instance.Pop(PoolType.Rune, runeSpawnPos) as Rune;
        runeData = _runeList.list[UnityEngine.Random.Range(0, _runeList.list.Count)];

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

            if (i == 3)
            {
                return false;
            }
        }

        CheckRuneSynergy(runeData.mythSynergyType);

        for (int i = 0; i < 2; ++i)
        {
            UIManager.Instance.TogglePopupUniquely("Inventory");
        }

        return true;
    }

    public bool TryUnequipRune(int index)
    {
        if (!_equipedRunes[index])
        {
            return false;
        }

        _equipedRunes[index] = null;

        for (int i = 0; i < 2; ++i)
        {
            UIManager.Instance.TogglePopupUniquely("Inventory");
        }

        return true;
    }

    public RuneDataSO GetEquipedRune(int index)
    {
        return _equipedRunes[index];
    }

    public int GetEquipedRuneIndex(RuneDataSO runeData)
    {
        Debug.Log("��");
        return _equipedRunes.IndexOf(runeData);
    }

    public void CheckRuneSynergy(BuffType runeSynergyType)
    {
        if (_equipedRunes.FindAll((runeData) => runeData && (runeData.mythSynergyType == runeSynergyType)).Count >= 3)
        {
            GameManager.Instance.PlayerInstance.BuffCompo.StopBuff(_synergizeRuneType);
            GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(runeSynergyType);

            _synergizeRuneType = runeSynergyType;
        }
        else if (_equipedRunes.FindAll((runeData) => runeData && (runeData.mythSynergyType == _synergizeRuneType)).Count < 3)
        {
            GameManager.Instance.PlayerInstance.BuffCompo.StopBuff(_synergizeRuneType);

            _synergizeRuneType = BuffType.None;
        }
    }

    public void RegisterRune(Rune rune)
    {
        if (_runes == null)
        {
            _runes = new List<Rune>();
        }
        _runes.Add(rune);
    }

    public void InitializeRunes()
    {
        if (_runes == null)
        {
            return;
        }

        foreach (Rune rune in _runes)
        {
            rune.InitializePoolItem();
        }
    }
}
