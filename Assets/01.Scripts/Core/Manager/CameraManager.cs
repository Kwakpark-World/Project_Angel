using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    private Dictionary<CameraType, CameraState> _cameraDictionary = new Dictionary<CameraType, CameraState>();
    public CameraState currentCam { get; private set; } = null;

    public void AddCamera(CameraState addCamera)
    {
        if (addCamera._type == CameraType.None)
        {
            Debug.LogError($"{addCamera} type is None. Select Camera Type");
        }

        _cameraDictionary.Add(addCamera._type, addCamera.RegisterCamera());
    }

    public void SetCamera(CameraState selectCam)
    {
        if (selectCam == null)
        {
            Debug.LogError($"CameraManager SetCam Error : {selectCam} is Not CameraState.");
            return;
        }

        currentCam?.UnSelectCamera();
        currentCam = selectCam;
        currentCam?.SelectCamera();
    }

    public CameraState GetCameraByType(CameraType type)
    {
        if (!_cameraDictionary.ContainsKey(type))
        {
            Debug.LogError($"{type} is not Contain. Register CameraState");
        }

        return _cameraDictionary[type];
    }

    public void CameraEventTrigger(CameraState camera)
    {
        if (camera == null)
        {
            Debug.LogError($"{camera} is Null");
            return;
        }

        camera?.CameraEvent();
    }

    // shake
    // zoom in, out
}
