using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    private static CinemachineVirtualCamera _cmVCam;
    public static CinemachineVirtualCamera CmVCam
    {
        get
        {
            if(_cmVCam == null )
            {
                _cmVCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
                if(_cmVCam == null )
                {
                    Debug.LogError("Cinemachine Virtual Camera doesn't exist.");
                }
            }
            return _cmVCam;
        }
    }
}
