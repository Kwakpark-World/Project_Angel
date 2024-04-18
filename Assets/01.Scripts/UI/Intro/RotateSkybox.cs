using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    [SerializeField]
    private float _rotateTime = 1;

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * _rotateTime);
    }
}
