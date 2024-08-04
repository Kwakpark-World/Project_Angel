using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

[System.Serializable]
struct FloorEffect
{
    //Color의 경우 Alpha값이 0이면 이펙트 안나옵니다 주의하십쇼
    //Effect Color
    public Color floorColor;
    public Color glowColor;
    public Color spriteColor;

    //Variable Texture
    public Texture texture;
}

public class FloorEffectSpriteChanger : MonoBehaviour
{
    private ParticleSystem _mainParticle;

    [Header("Debug")]
    [SerializeField] private int _particleStructIndex;

    [Header("Components")]
    [SerializeField] private ParticleSystem _floorParticle;
    [SerializeField] private ParticleSystem _glowParticle;
    [SerializeField] private ParticleSystem _spriteParticle;

    [Header("Need to Material")]
    [SerializeField] private FloorEffect[] _particleStruct;
    [SerializeField] private string _nameID = "_main_tex";


    private void Awake()
    {
        _mainParticle = GetComponent<ParticleSystem>();

        //Debug
        SetEffect(_particleStructIndex);
    }

    /// <summary>
    /// 이펙트 실행 시 어떤 스프라이트를 사용하는지 세팅하는 친구
    /// </summary>
    /// <param name="index">인덱스에 따라 세팅되는 이펙트의 색상, 스프라이트가 결정된다</param>
    public void SetEffect(int index)
    {
        if(_floorParticle == null || _spriteParticle == null || _particleStruct == null)
        {
            Debug.LogError("some variables is null");
            return;
        }

        var floorMain = _floorParticle.main;
        var glowMain = _glowParticle.main;
        var spriteMain = _spriteParticle.main;
        var spriteRenderer = _spriteParticle.GetComponent<ParticleSystemRenderer>();

        floorMain.startColor = _particleStruct[index].floorColor;
        glowMain.startColor = _particleStruct[index].glowColor;
        spriteMain.startColor = _particleStruct[index].spriteColor;
        spriteRenderer.material.SetTexture(_nameID, _particleStruct[index].texture);

        _mainParticle.Play();
    }
}
