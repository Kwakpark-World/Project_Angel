using Cinemachine;
using System;
using UnityEngine;

public class CameraState : MonoBehaviour
{
    public CinemachineVirtualCameraBase _camera { get; private set; }
    public CameraType _type;

    protected event Action _cameraEvent;

    public virtual CameraState RegisterCamera()
    {
        _camera = GetComponent<CinemachineVirtualCameraBase>();
        _camera.Priority = 0;

        if (_camera == null)
        {
            Debug.LogError($"CameraState Init Error : {gameObject.name} is not Cinemachine. Value Return Null. => Attatch Component VirtualCam");
        }
        
        // Add Event In Child Component

        return this;
    }

    public void CameraEvent()
    {
        if (_cameraEvent == null)
        {
            Debug.Log("_camerEvent is Null. Subscribe event to _cameraEvent");
        }

        _cameraEvent?.Invoke();
    }

    public void SelectCamera()
    {
        _camera.Priority = 10;
    }
    
    public void UnSelectCamera()
    {
        _camera.Priority = 0;
    }

    public void LookAndFollowCamera(Transform lookTrm, Transform followTrm)
    {
        _camera.LookAt = lookTrm;
        _camera.Follow = followTrm;
    }
}
