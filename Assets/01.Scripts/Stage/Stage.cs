using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Transform _environmentTrm;
    [SerializeField] private List<Barrier> _barriers = new List<Barrier>();

    private bool _running = false;

    private void Awake()
    {
        for(int i = 0; i < _barriers.Count; i++)
        {
            _barriers[i].Hide();
        }
    }

    public void StartStage()
    {
        _running = true;
        StartCoroutine(LockStage());
    }

    private IEnumerator LockStage()
    {
        GameManager.Instance.player.StopImmediately(true);

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
        if(other.CompareTag("Player"))
        {
            StartStage();
        }
    }
}
