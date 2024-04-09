using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Color _originColor;
    
    private WaitForSeconds _blinkDelay = new WaitForSeconds(0.15f);

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originColor = _meshRenderer.material.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(OnBlinkEffect());
        }
    }

    private IEnumerator OnBlinkEffect()
    {
        _meshRenderer.material.color = Color.white;
        yield return _blinkDelay;
        _meshRenderer.material.color = _originColor;
    }
}
