using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
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
    public bool IsYReverse;

    public void AddCamera(CameraState addCamera)
    {
        if (addCamera._type == CameraType.None)
        {

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

    public void ChangeOrbitBody()
    {
        if (_currentCam._camera.TryGetComponent<CinemachineVirtualCamera>(out CinemachineVirtualCamera cam))
        {
            var body = cam.AddCinemachineComponent<CinemachineOrbitalTransposer>();
            if (body)
            {
                GameManager.Instance.PlayerInstance.IsAttack = true;
                if (_currentCam.IsCamRotateStop)
                    body.m_XAxis.m_MaxSpeed = 0;
                else
                    body.m_XAxis.m_MaxSpeed = GameManager.Instance.PlayerInstance.PlayerStatData.GetXRotateSpeed() * 10;

                body.m_FollowOffset = new Vector3(0, 6.9f, -13f);
                body.m_XDamping = 0;
                body.m_YDamping = 0;
                body.m_ZDamping = 1;

                body.m_XAxis.m_InvertInput = IsXReverse;
            }
        }
    }

    public void Change3rdPersonBody()
    {

        Vector3 rot = CameraManager.Instance._currentCam._camera.transform.eulerAngles;
        rot.x = 0;
        rot.z = 0;
        GameManager.Instance.PlayerInstance.IsAttack = false;

        GameManager.Instance.PlayerInstance.transform.rotation = Quaternion.Euler(rot);

        if (_currentCam._camera.TryGetComponent<CinemachineVirtualCamera>(out CinemachineVirtualCamera cam))
        {
            var body = cam.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
            if (body)
            {
                body.Damping = new Vector3(0, 0, 1);
                body.ShoulderOffset = new Vector3(0, 6.5f, 0);
                body.CameraDistance = 13f;
                body.CameraSide = 1;
                body.CameraRadius = 0.2f;
                body.DampingFromCollision = 2f;
                body.VerticalArmLength = 0.4f;
            }
        }
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
