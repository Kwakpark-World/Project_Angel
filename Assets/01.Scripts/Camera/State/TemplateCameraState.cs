using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateCameraState : CameraState
{
    public override CameraState RegisterCamera()
    {
        base.RegisterCamera();
        _cameraEvent += TestEvent;
        
        return this;
    }

    private void Start()
    {
        CameraManager.Instance.AddCamera(this);
    }

    private void Update()
    {
        /* Debug
        if (Input.GetKeyDown(KeyCode.T))
        {
            CameraManager.Instance.AddCamera(this);

            CameraManager.Instance.SetCamera(this);
        }

        if (Input.GetKeyDown (KeyCode.B))
        {
            CameraManager.Instance.CameraEventTrigger();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(CameraManager.Instance.GetCameraByType(CameraType.PlayerCam));
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            CameraManager.Instance.ShakeCam(5f, 1f, 1f);
        }

        
        CameraManager.Instance.ZoomCam(0, 70, GameManager.Instance.PlayerInstance.PlayerInput.MouseWheel);
        */
    }

    public void TestEvent()
    {
        Debug.Log("CameraTemplate : CameraEvnet TestingCode");
    }
}
