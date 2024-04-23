using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stage : MonoBehaviour
{
    [SerializeField] private List<Barrier> _barriers = new List<Barrier>();
    [SerializeField] private Transform _runeSpawnTrm;

    private bool _running = false;
    private bool _isClear = false;

    private void Awake()
    {
        for(int i = 0; i < _barriers.Count; i++)
        {
            _barriers[i].Hide();
        }
    }

    private void Update()
    {
#if UNITY_EDITOR // Debug
        if (_running && Keyboard.current.cKey.wasPressedThisFrame)
        {
            ClearStage();
        }
#endif
    }

    public void StartStage()
    {
        _running = true;

        StartCoroutine(LockStage());
        SoundManager.Instance.ChangeBGMMode(BGMMode.Combat);
    }

    public void ClearStage()
    {
        _running = false;
        _isClear = true;

        StartCoroutine(UnlockStage());
        SoundManager.Instance.ChangeBGMMode(BGMMode.NonCombat);
        RuneManager.Instance.CreateRune(_runeSpawnTrm.position);
    }

    private IEnumerator UnlockStage()
    {
        GameManager.Instance.PlayerInstance.StopImmediately(true);

        // TODO: disable player input
        for (int i = 0; i < _barriers.Count; i++)
        {
            _barriers[i].Hide();
        }

        yield return new WaitForSeconds(1f);

        // TODO: enable player input
    }

    private IEnumerator LockStage()
    {
        GameManager.Instance.PlayerInstance.StopImmediately(true);

        // TODO: disable player input
        for (int i = 0; i < _barriers.Count; i++)
        {
            _barriers[i].Show();
        }

        yield return new WaitForSeconds(1f);

        // TODO: enable player input
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !_isClear)
        {
            StartStage();
        }
    }
}
