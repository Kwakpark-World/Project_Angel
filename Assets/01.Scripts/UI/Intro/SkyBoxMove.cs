using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxMove : MonoBehaviour
{
    public float RotateTime = 1;

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotateTime);
    }
}
