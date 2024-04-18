using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuneManager : MonoSingleton<RuneManager>
{
    [SerializeField]
    private RuneListSO _runeList;

    public Dictionary<RuneType, List<Rune>> collectedRunes = new Dictionary<RuneType, List<Rune>>();
    public bool isLastDance = false;
    public bool isDebuff = false;


    private void Awake()
    {
        foreach (RuneType type in Enum.GetValues(typeof(RuneType)))
        {
            collectedRunes[type] = new List<Rune>();
        }
    }

    public void Update()
    {
#if UNITY_EDITOR // Debug
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            CreateRune(GameManager.Instance.PlayerInstance.transform.position + Vector3.forward * 5f);
        }
#endif
    }

    public void SetRuneList(RuneListSO list)
    {
        _runeList = list;
    }

    public Rune CreateRune(Vector3 runeSpawnPos)
    {
        Rune rune = PoolManager.Instance.Pop(PoolingType.Rune, runeSpawnPos) as Rune;
        RuneDataSO runeData = _runeList.list[UnityEngine.Random.Range(0, _runeList.list.Count)];

        rune.SetRuneData(runeData);
        rune.InitializeRune();
        _runeList.list.Remove(runeData);

        return rune;
    }

    public void CheckRuneSynergy()
    {
        foreach (var rune in collectedRunes)
        {
            if (rune.Value.Count < 3)
            {
                return;
            }

            switch (rune.Key)
            {
                case RuneType.Attack:
                    break;

                case RuneType.Defense:
                    isDebuff = false;

                    break;

                case RuneType.Health:
                    Debug.Log("3");
                    StartCoroutine(LastDance(5f));

                    break;

                case RuneType.Acceleration:
                    break;

                case RuneType.Debuff:
                    if (UnityEngine.Random.value <= 0.5f)
                    {
                        //준호가 공격 만들면 여기다가 할 예정
                    }

                    break;

                default:
                    break;
            }
        }
    }

    private IEnumerator LastDance(float time)
    {
        if (GameManager.Instance.PlayerInstance.CurrentHealth <= 1 && isLastDance == false)
        {
            isLastDance = true;
        }

        yield return new WaitForSeconds(time);

        isLastDance = false;
    }
}
