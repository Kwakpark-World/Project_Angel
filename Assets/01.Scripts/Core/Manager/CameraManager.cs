using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    private Dictionary<CameraType, CameraState> _cameraDictionary = new Dictionary<CameraType, CameraState>();
    public CameraState _currentCam { get; private set; } = null;

    [SerializeField]
    private NoiseSettings shake6DSettings;

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

        _currentCam?.UnSelectCamera();
        _currentCam = selectCam;
        _currentCam?.SelectCamera();
    }

    public CameraState GetCameraByType(CameraType type)
    {
        if (!_cameraDictionary.ContainsKey(type))
        {
            Debug.LogError($"{type} is not Contain. Register CameraState");
        }

        return _cameraDictionary[type];
    }

    public void CameraEventTrigger()
    {
        if (_currentCam == null)
        {
            Debug.LogError($"CurrentCamera is Null");
            return;
        }

        _currentCam?.CameraEvent();
    }

    private Coroutine _ShakeCameraCoroutine = null;
    public void ShakeCam(float duration, float frequency, float amplitude)
    {
        if (_currentCam == null)
        {
            Debug.LogError("Current Camera is Null");
        }

        if (_ShakeCameraCoroutine != null)
            StopCoroutine(ShakeCamera(duration, frequency, amplitude));


        _ShakeCameraCoroutine = StartCoroutine(ShakeCamera(duration, frequency, amplitude));
    }

    public void ZoomCam(float minZoom, float maxZoom, float addValue)
    {
        if (_currentCam == null)
        {
            Debug.LogError("CurrentCamera is Null");
            return;
        }
        CinemachineVirtualCamera vCam = _currentCam._camera.GetComponent<CinemachineVirtualCamera>();

        StartCoroutine(ZoomCamera(vCam, minZoom, maxZoom, addValue));
    }

    private IEnumerator ShakeCamera(float duration, float frequency, float amplitude)
    {
        CinemachineVirtualCamera noiseCamera = _currentCam.GetComponent<CinemachineVirtualCamera>();
        if (noiseCamera == null)
        {
            Debug.LogError($"{noiseCamera} is Not Virtual Camera");
        }

        CinemachineBasicMultiChannelPerlin noise = noiseCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null)
        {
            noiseCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise = noiseCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (shake6DSettings == null)
            {
                Debug.LogError("6D Shake Asset is NUll");
            }

            noise.m_NoiseProfile = shake6DSettings;
        }
        noise.m_FrequencyGain = frequency;
        noise.m_AmplitudeGain = amplitude;

        yield return new WaitForSeconds(duration);

        noise.m_FrequencyGain = 0;
        noise.m_AmplitudeGain = 0;
    }    

    private IEnumerator ZoomCamera(CinemachineVirtualCamera vCam, float minZoom, float maxZoom, float addValue)
    {
        float whileValue = Mathf.Abs(addValue);

        while (whileValue > 0f)
        {
            vCam.m_Lens.OrthographicSize = Mathf.Clamp(vCam.m_Lens.OrthographicSize, minZoom, maxZoom);
            vCam.m_Lens.OrthographicSize += addValue * Time.deltaTime;
            whileValue -= Mathf.Abs(addValue) * Time.deltaTime;   
            yield return null;
        }
    }
}
