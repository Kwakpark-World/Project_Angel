using Cinemachine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraManager : MonoSingleton<CameraManager>
{
    private Dictionary<CameraType, CameraState> _cameraDictionary = new Dictionary<CameraType, CameraState>();
    public CameraState _currentCam { get; private set; } = null;

    [SerializeField]
    private NoiseSettings shake6DSettings;

    private float DefaultOrthoSize;
    public bool IsXReverse;

    public void AddCamera(CameraState addCamera)
    {
        if (addCamera._type == CameraType.None)
        {
            Debug.LogError($"{addCamera} type is None. Select Camera Type");
        }

        if (_cameraDictionary.ContainsKey(addCamera._type))
        {
            _cameraDictionary.Remove(addCamera._type);
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

        _currentCam._camera.TryGetComponent<CinemachineVirtualCamera>(out CinemachineVirtualCamera vCam);
        if (vCam != null)
            DefaultOrthoSize = vCam.m_Lens.OrthographicSize;


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

    private Coroutine _shakeCameraCoroutine = null;
    public void ShakeCam(float duration, float frequency, float amplitude)
    {
        if (_currentCam == null)
        {
            Debug.LogError("Current Camera is Null");
        }

        if (_shakeCameraCoroutine != null)
            StopCoroutine(_shakeCameraCoroutine);


        _shakeCameraCoroutine = StartCoroutine(ShakeCamera(duration, frequency, amplitude));
    }

    private bool _isZoomStop = false;

    public void ZoomCam(float addValuePerTick, float minZoom = 6, float maxZoom = 7.5f)
    {
        _isZoomStop = false;
        if (_currentCam == null)
        {
            Debug.LogError("CurrentCamera is Null");
            return;
        }

        CinemachineVirtualCamera vCam = _currentCam._camera.GetComponent<CinemachineVirtualCamera>();

        StartCoroutine(ZoomCamera(vCam, minZoom, maxZoom, addValuePerTick));
    }

    public void ZoomCam(float targetValue, float changeValuePerTick)
    {
        _isZoomStop = false;
        if (_currentCam == null)
        {
            Debug.LogError("CurrentCamera is Null");
            return;
        }

        CinemachineVirtualCamera vCam = _currentCam._camera.GetComponent<CinemachineVirtualCamera>();

        StartCoroutine(ZoomCamera(vCam, targetValue, changeValuePerTick));
    }

    public void StopZoomCam()
    {
        _isZoomStop = true;
    }

    public void ResetCameraZoom()
    {
        CinemachineVirtualCamera vCam = _currentCam._camera.GetComponent<CinemachineVirtualCamera>();

        StopZoomCam();

        vCam.m_Lens.OrthographicSize = DefaultOrthoSize;
    }

    // 커서 키면 보이고 락풀리고, 커서 끄면 안보이고 락걸리고.
    public void SetCursor(bool isOnCursor)
    {
        Cursor.visible = isOnCursor;
        Cursor.lockState = isOnCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // 시간, 빈도, 진폭
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

    private IEnumerator ZoomCamera(CinemachineVirtualCamera vCam, float minZoom, float maxZoom, float addValuePerTick)
    {
        float whileValue = Mathf.Abs(addValuePerTick);

        while (whileValue > 0f)
        {
            if (_isZoomStop) break;

            vCam.m_Lens.OrthographicSize = Mathf.Clamp(vCam.m_Lens.OrthographicSize, minZoom, maxZoom);
            vCam.m_Lens.OrthographicSize += addValuePerTick * Time.deltaTime;
            whileValue -= Mathf.Abs(addValuePerTick) * Time.deltaTime;   
            yield return null;
        }
    }

    private IEnumerator ZoomCamera(CinemachineVirtualCamera vCam, float targetValue, float changeValuePerTick)
    {

        while (!Mathf.Approximately(vCam.m_Lens.OrthographicSize, targetValue))
        {
            if (_isZoomStop) break;

            vCam.m_Lens.OrthographicSize = Mathf.MoveTowards(vCam.m_Lens.OrthographicSize, targetValue, changeValuePerTick * Time.deltaTime);
            yield return null;
        }
    }

}
