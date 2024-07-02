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
            GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_runeData.buffType);
            transform.Find(_runeData.name).gameObject.SetActive(false);
            PoolManager.Instance.Push(this);
        }
        /* CameraManager.Instance.StopZoomCam();*/
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
