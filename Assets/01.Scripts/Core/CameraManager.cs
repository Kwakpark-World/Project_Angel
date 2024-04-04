using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    public float shakeElapsedTime;

    private Coroutine shakeCoroutine;

    Dictionary<string, CinemachineVirtualCameraBase> _cameraDictionary = new Dictionary<string, CinemachineVirtualCameraBase>(); // 리플렉션으로 값 다 가져오기
    private CinemachineVirtualCameraBase currentCam = null;

    public void AddCam(CinemachineVirtualCameraBase addCam)
    {
        _cameraDictionary.Add(addCam.name, addCam);
    }

    public void FindCamByString(string camName)
    {

    }

    public void FindCamByCinemachine(CinemachineVirtualCameraBase findCam)
    {

    }

    public void SetCam(CinemachineVirtualCameraBase selectCam)
    {
        if (currentCam == null)
        {
            currentCam = selectCam;
        }

        currentCam.Priority = 0;
        selectCam.Priority = 10;
        currentCam = selectCam;
    }

    public void ShakeCam(float shakeDuration, float shakeAmplitude, float shakeFrequency)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeCamCoroutine(shakeDuration, shakeAmplitude, shakeFrequency));
    }

    private IEnumerator ShakeCamCoroutine(float shakeDuration, float shakeAmplitude, float shakeFrequency)
    {
        CinemachineBasicMultiChannelPerlin _virtualCameraNoise = null;

        if (currentCam != null)
        {
            _virtualCameraNoise = currentCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        shakeElapsedTime = shakeDuration;

        while (shakeElapsedTime > 0)
        {
            _virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
            _virtualCameraNoise.m_FrequencyGain = shakeFrequency;

            shakeElapsedTime -= Time.deltaTime;

            yield return null;
        }
        _virtualCameraNoise.m_AmplitudeGain = 0f;
        shakeElapsedTime = 0f;
    }
}
