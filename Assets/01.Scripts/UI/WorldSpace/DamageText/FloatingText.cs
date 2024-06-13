using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SymbolsTextureData
{
    public Texture texture;
    public char[] characters;
    private Dictionary<char, Vector2> _charactersPosition;

    public void Initialize()
    {
        _charactersPosition = new Dictionary<char, Vector2>();

        for (int i = 0; i < characters.Length; ++i)
        {
            var c = char.ToLowerInvariant(characters[i]);

            if (_charactersPosition.ContainsKey(c))
            {
                continue;
            }

            var uv = new Vector2(i % 10, 9 - i / 10);

            _charactersPosition.Add(c, uv);
        }
    }

    public Vector2 GetTextureCoordinates(char c)
    {
        c = char.ToLowerInvariant(c);

        if (_charactersPosition == null)
        {
            Initialize();
        }

        if (_charactersPosition.TryGetValue(c, out Vector2 texCoord))
        {
            return texCoord;
        }

        return Vector2.zero;
    }
}

[RequireComponent(typeof(ParticleSystem))]
public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private SymbolsTextureData _textureData;
    private ParticleSystemRenderer _particleSystemRenderer;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void SpawnParticle(Vector3 position, string message, Color color, float? startSize = null)
    {
        var texCoords = new Vector2[24];
        var messageLength = Mathf.Min(23, message.Length);
        texCoords[texCoords.Length - 1] = new Vector2(0, messageLength);

        for (int i = 0; i < texCoords.Length; ++i)
        {
            if (i >= messageLength)
            {
                break;
            }

            texCoords[i] = _textureData.GetTextureCoordinates(message[i]);
        }

        var custom1Data = CreateCustomData(texCoords);
        var custom2Data = CreateCustomData(texCoords, 12);
        var emitParams = new ParticleSystem.EmitParams
        {
            startColor = color,
            position = position,
            applyShapeToPosition = true,
            startSize3D = new Vector3(messageLength, 1, 1)
        };

        if (startSize.HasValue)
        {
            emitParams.startSize3D *= startSize.Value * _particleSystem.main.startSizeMultiplier;
        }

        _particleSystem.Emit(emitParams, 1);

        var customData = new List<Vector4>();

        _particleSystem.GetCustomParticleData(customData, ParticleSystemCustomData.Custom1);

        customData[customData.Count - 1] = custom1Data;

        _particleSystem.SetCustomParticleData(customData, ParticleSystemCustomData.Custom1);
        _particleSystem.GetCustomParticleData(customData, ParticleSystemCustomData.Custom2);

        customData[customData.Count - 1] = custom2Data;

        _particleSystem.SetCustomParticleData(customData, ParticleSystemCustomData.Custom2);
    }

    public float PackFloat(Vector2[] vecs)
    {
        if (vecs == null || vecs.Length == 0)
        {
            return 0;
        }

        var result = vecs[0].y * 10000 + vecs[0].x * 100000;

        if (vecs.Length > 1)
        {
            result += vecs[1].y * 100 + vecs[1].x * 1000;
        }

        if (vecs.Length > 2)
        {
            result += vecs[2].y + vecs[2].x * 10;
        }

        return result;
    }

    private Vector4 CreateCustomData(Vector2[] texCoords, int offset = 0)
    {
        var data = Vector4.zero;

        for (int i = 0; i < 4; ++i)
        {
            var vecs = new Vector2[3];

            for (int j = 0; j < 3; ++j)
            {
                var ind = i * 3 + j + offset;

                if (texCoords.Length > ind)
                {
                    vecs[j] = texCoords[ind];
                }
                else
                {
                    data[i] = PackFloat(vecs);
                    i = 5;

                    break;
                }
            }

            if (i < 4)
            {
                data[i] = PackFloat(vecs);
            }
        }

        return data;
    }
}
