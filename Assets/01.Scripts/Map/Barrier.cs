using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private BoxCollider _boxCollider;

    private Material _material;

    private bool _visible = false;
    private bool _changing = false;

    private const string ALPHA_PROP = "_main_alpha_mul";

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _boxCollider = GetComponent<BoxCollider>();

        _material = new Material(_meshRenderer.material);
        _meshRenderer.material = _material;

        _material.SetFloat(ALPHA_PROP, 0f);
        _boxCollider.enabled = false;
    }

    public void Show(float time = 1f)
    {
        UIManager.Instance._mode = BGMMode.Combat;
        if (_visible) return;
        if(!_changing)
        {
            _boxCollider.enabled = true;
            _changing = true;
            DOTween.To(() => _material.GetFloat(ALPHA_PROP), (x) => _material.SetFloat(ALPHA_PROP, x), 0.78f, time)
                   .OnComplete(() =>
                   {
                       _changing = false;
                       _visible = true;
                   });
        }
    }

    public void Hide(float time = 1f)
    {
        UIManager.Instance._mode = BGMMode.NonCombat;
        if (!_visible) return;
        if (!_changing)
        {
            _changing = true;
            DOTween.To(() => _material.GetFloat(ALPHA_PROP), (x) => _material.SetFloat(ALPHA_PROP, x), 0f, time)
                   .OnComplete(() =>
                   {
                       _boxCollider.enabled = false;
                       _changing = false;
                       _visible = false;
                   });
        }
    }
}
