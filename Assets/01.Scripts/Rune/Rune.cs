using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rune : PoolableMono
{
    [Header("Light Properties")]
    [SerializeField]
    private float _floatingHeight = 0.75f;
    [SerializeField]
    private float _floatingSpeed = 1.5f;
    [SerializeField]
    private float _rotateSpeed = 1.5f;
    [SerializeField]
    private Light _runeLight;
    [SerializeField]
    private float nearPlayer = 5f;

    public ParticleSystem runeParticle;

    private RuneDataSO _runeData;
    public RuneDataSO RuneData
    {
        get
        {
            if (_runeData == null)
            {
                Debug.LogError($"Rune data is null.");
            }

            return _runeData;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Interactive();
        }
    }

    private void Interactive()
    {
        float distancePlayer = Vector3.Distance(gameObject.transform.position, GameManager.Instance.PlayerInstance.transform.position);

        if (distancePlayer <= nearPlayer)
        {
            if (!RuneManager.Instance.TryEquipRune(RuneData))
            {
                return;
            }

            StopAllCoroutines();

            runeParticle.Play();

            // 파티클이 끝난 후 실행될 코루틴 시작
            StartCoroutine(WaitForParticleToFinish());
        }
        /* CameraManager.Instance.StopZoomCam();*/
    }

    private IEnumerator WaitForParticleToFinish()
    {
        // 파티클이 실행 중인 동안 기다림
        while (runeParticle.isPlaying)
        {
            Debug.Log("실행중");
            yield return null;
        }
        GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_runeData.buffType);
        transform.Find(_runeData.name).gameObject.SetActive(false);
        PoolManager.Instance.Push(this);
    }

    public override void InitializePoolItem()
    {
        _runeData.IsDestroyed();
    }

    public void InitializeRune(RuneDataSO runeData)
    {
        _runeData = runeData;
        _runeLight.color = runeData.runeColor;

        transform.Find(runeData.name).gameObject.SetActive(true);
        StartCoroutine(FloatingCoroutine());
    }

    #region Coroutine
    private IEnumerator FloatingCoroutine()
    {
        Vector3 pos = transform.position;
        float originY = pos.y;
        float t = 0f;

        while (true)
        {
            pos.y = Mathf.Sin(t) * _floatingHeight + originY + _floatingHeight;
            transform.position = pos;
            t += Time.deltaTime * _floatingSpeed;

            yield return null;
        }
    }
    #endregion
}
