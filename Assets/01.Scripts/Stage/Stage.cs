using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stage : MonoBehaviour
{
    [SerializeField] private Transform _environmentTrm;
    [SerializeField] private Transform _runeSpawnTrm;
    [SerializeField] private List<Barrier> _barriers = new List<Barrier>();

    [SerializeField] private CinemachineVirtualCamera _stageCam;

    private bool _running = false;
    private bool _isClear = false;

    private void Awake()
    {
        for(int i = 0; i < _barriers.Count; i++)
        {
            _barriers[i].Hide();
        }
    }

    // This is for Debug
    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            ClearStage();
        }
    }

    public void StartStage(Player player)
    {
        _running = true;
        StartCoroutine(LockStage());

        //TODO: Active Scene Cam
        _stageCam.Priority = 15;
        //_stageCam.Follow = player.transform;
    }

    public void ClearStage()
    {
        _running = false;
        _isClear = true;
        StartCoroutine(UnlockStage());

        _stageCam.Priority = 0;

        Rune r = RuneManager.Instance.CreateRune();
        r.transform.position = _runeSpawnTrm.position;
        r.StartFloating();
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
            StartStage(other.GetComponent<Player>());
        }
    }
}
