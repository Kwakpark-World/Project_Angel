using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _shakeTime = 1.0f;
    [SerializeField] private float _shakeIntensity = 0.1f;
    [SerializeField] private GameObject _camera;
    [SerializeField] public CinemachineVirtualCameraBase _playerCamera;


    private void Awake()
    {
        //CameraManager.Instance
        CameraManager.Instance.AddCam(_playerCamera);
        _camera = GameObject.Find("CameraPosition");
    }

    private void Start()
    {
        CameraManager.Instance.SetCam(_playerCamera);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            CameraManager.Instance.ShakeCam(0.3f, 1.2f, 2.0f);
        }
    }
}
