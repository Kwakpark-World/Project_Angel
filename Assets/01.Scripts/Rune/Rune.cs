using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RuneType
{
    Attack,
    Defense,
    Health,
    Acceleration,
    Debuff,
    End
}

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
    private float _lightIntensity = 5f;
    [SerializeField]
    private Light _pointLight;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player p))
        {
            StopAllCoroutines();

            RuneType runtype = _runeData.runeType;
            Sprite runeSprite = _runeData.runeSprite;

            RuneInventory.Instance.AddItem(runtype, runeSprite);
           

            if (_runeData == null)
            {
                Debug.LogError($"Rune data is null.");

                return;
            }

            p.BuffCompo.SetBuff(_runeData.buffType, this);
            RuneManager.Instance.collectedRunes[_runeData.runeType].Add(this);
            
            RuneManager.Instance.CheckRuneSynergy();
            PoolManager.Instance.Push(this);
        }
    }

    public override void InitializePoolingItem()
    {
        _runeData = null;
    }

    public void InitializeRune()
    {
        Debug.Log(_runeData.name);
        transform.Find(_runeData.name).gameObject.SetActive(true);
        StartCoroutine(FloatingCoroutine());
    }

    public void SetRuneData(RuneDataSO runeData)
    {
        _runeData = runeData;
        _pointLight.color = _runeData.lightColor;
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
