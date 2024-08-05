using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

[System.Serializable]
struct FloorEffect
{
    //Color�� ��� Alpha���� 0�̸� ����Ʈ �ȳ��ɴϴ� �����Ͻʼ�
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
    /// ����Ʈ ���� �� � ��������Ʈ�� ����ϴ��� �����ϴ� ģ��
    /// </summary>
    /// <param name="index">�ε����� ���� ���õǴ� ����Ʈ�� ����, ��������Ʈ�� �����ȴ�</param>
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
