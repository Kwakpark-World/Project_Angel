using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RuneType
{
    STRENGTH,   // 힘, 공격
    ARMOR,      // 방어력
    HEALTH,     // 체력
    DEXTERITY,  // 민첩, 스피드
    DEBUFF,     // 디버프
    END
}

[RequireComponent(typeof(SpriteRenderer))]
public class Rune : PoolableMono
{
    [Header("Light Properties")]
    [SerializeField] private float _floatingHeight = 0.75f;
    [SerializeField] private float _floatingSpeed = 1.5f;
    [SerializeField] private float _lightIntensity = 5f;
    [SerializeField] private Light _pointLight;

    private RuneEffectSO _runeData;
    public RuneEffectSO RuneData
    {
        get
        {
            if(_runeData == null)
            {
                Debug.LogError($"RuneData is Null. GameObject: {gameObject.name}");
            }
            return _runeData;
        }
    }

    private SpriteRenderer _spriteRenderer;
    private Material _material;

    private readonly int _dissolveValueHash = Shader.PropertyToID("_Value");

    private void Awake() 
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
        _material.SetFloat(_dissolveValueHash, 0f);
    }


    private void OnTriggerEnter(Collider other)
    {
        //TODO: 룬 수집 시 애니메이션 및 연출
        if(other.TryGetComponent(out Player p))
        {
            StopAllCoroutines();

            if(_runeData == null)
            {
                Debug.LogError($"RuneData is Null. GameObject: {gameObject.name}");
                return;
            }
            _runeData.UseEffect();
            PoolManager.Instance.Push(this);
            RuneManager.Instance.ActivateRune();
        }
    }

    #region Coroutine

    private IEnumerator DissolveCoroutine(float target, float duration = 2f)
    {
        target = Mathf.Clamp(target, 0f, 1f);
        float startValue = _material.GetFloat(_dissolveValueHash);
        float t = 0f;
        while(t <= duration)
        {
            float value = Mathf.Lerp(startValue, target, t / duration);
            _material.SetFloat(_dissolveValueHash, value);
            _pointLight.intensity = value * _lightIntensity;
            t += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FloatingCoroutine()
    {
        Vector3 pos = transform.position;
        float originY = pos.y;
        float t = 0f;
        while(true)
        {
            pos.y = Mathf.Sin(t) * _floatingHeight + originY;
            transform.position = pos;
            t += Time.deltaTime * _floatingSpeed;
            yield return null;
        }
    }

    #endregion

    public override void InitializePoolingItem()
    {
        _material.SetFloat(_dissolveValueHash, 0f);
        _runeData = null;
    }

    public void SetDissolve(float target)
    {
        StartCoroutine(DissolveCoroutine(target));
    }

    public void StartFloating()
    {
        StartCoroutine(FloatingCoroutine());
    }

    public void SetRuneData(RuneEffectSO runeData)
    {
        _runeData = runeData;
        _spriteRenderer.sprite = _runeData.runeSprite;
        _pointLight.color = _runeData.lightColor;
    }

    public void EffectsRuneSynergy()
    {
        
    }
}
