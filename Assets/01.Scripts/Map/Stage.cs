using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stage : MonoBehaviour
{
    [SerializeField] private List<Barrier> _barriers = new List<Barrier>();
    [SerializeField] private List<EnemyMannequin> _mannequins = new List<EnemyMannequin>();
    [SerializeField] private Transform _runeSpawnTrm;

    private bool _running = false;
    private bool _isClear = false;

    private void Awake()
    {
        for (int i = 0; i < _barriers.Count; i++)
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
        foreach (EnemyMannequin mannequin in _mannequins)
        {
            if (mannequin.gameObject.activeInHierarchy)
            {
                return;
            }
        }

        ClearStage();
    }

    public void StartStage()
    {
        SoundManager.Instance.ChangeBGMMode(BGMMode.Combat);

        GameManager.Instance.PlayerInstance.Navigation.PathLine.enabled = false;
        _running = true;

        foreach (EnemyMannequin mannequin in _mannequins)
        {
            mannequin.SpawnEnemy();
        }

        StartCoroutine(LockStage());
    }

    public void ClearStage()
    {
        SoundManager.Instance.ChangeBGMMode(BGMMode.NonCombat);

        GameManager.Instance.PlayerInstance.Navigation.PathLine.enabled = true;
        _running = false;
        _isClear = true;

        StartCoroutine(UnlockStage());
    }

    private IEnumerator UnlockStage()
    {
        GameManager.Instance.PlayerInstance.StopImmediately(true);

        GameManager.Instance.PlayerInstance.IsPlayerStop = PlayerControlEnum.Stop;

        for (int i = 0; i < _barriers.Count; i++)
        {
            _barriers[i].Hide();
        }

        yield return new WaitForSeconds(1f);

        RuneManager.Instance.SpawnRune(_runeSpawnTrm.position);
        gameObject.SetActive(false);

        GameManager.Instance.PlayerInstance.IsPlayerStop = PlayerControlEnum.Move;
    }

    private IEnumerator LockStage()
    {
        GameManager.Instance.PlayerInstance.StopImmediately(true);

        GameManager.Instance.PlayerInstance.IsPlayerStop = PlayerControlEnum.Stop;

        for (int i = 0; i < _barriers.Count; i++)
        {
            _barriers[i].Show();
        }

        yield return new WaitForSeconds(1f);

        SoundManager.Instance.ChangeBGMMode(BGMMode.Combat);

        GameManager.Instance.PlayerInstance.IsPlayerStop = PlayerControlEnum.Move;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isClear && !_running)
        {
            StartStage();
        }
    }
}
