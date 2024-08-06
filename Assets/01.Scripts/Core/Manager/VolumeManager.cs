using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class VolumeManager : MonoSingleton<VolumeManager>
{
    private Volume volume;
    private MotionBlur motionBlur;

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        volume = FindObjectOfType<Volume>();

        if (volume != null && volume.profile != null)
        {
            // Volume 프로파일에서 모든 설정을 가져옴
            foreach (var component in volume.profile.components)
            {
                // MotionBlur 타입인지 확인하고 가져옴
                if (component is MotionBlur mb)
                {
                    motionBlur = mb;
                    Debug.Log("Motion Blur component found");
                    break;
                }
            }

            if (motionBlur == null)
            {
                Debug.LogError("Motion Blur component not found in Volume profile");
            }
        }
        else
        {
            Debug.LogError("Volume or Volume profile is not assigned");
        }
    }

    private void Update()
    {
        if (motionBlur != null)
        {
            //Debug.Log(motionBlur.intensity.value);
        }
    }

    public IEnumerator HitMotionBlur(float duration, float intensity)
    {
        if (motionBlur != null)
        {
            float originalIntensity = motionBlur.intensity.value;
            float elapsedTime = 0f;
            while (elapsedTime < duration) 
            {
                motionBlur.intensity.value = Mathf.Lerp(originalIntensity, intensity, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                motionBlur.intensity.value = Mathf.Lerp(intensity, originalIntensity, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            motionBlur.intensity.value = originalIntensity;
        }
    }
}
