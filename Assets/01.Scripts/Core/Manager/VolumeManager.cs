using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeManager : MonoSingleton<VolumeManager>
{
    public PostProcessVolume postProcessVolume;
    private MotionBlur motionBlur;

    private void Update()
    {
        postProcessVolume.profile.TryGetSettings(out motionBlur);
        Debug.Log(motionBlur);

        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("2");
            motionBlur.shutterAngle.value += 1;
        }
    }

    public IEnumerator HitMotionBlur(float duration, float intensity)
    {
        if (motionBlur != null)
        {
            float originalIntensity = motionBlur.shutterAngle.value;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                motionBlur.shutterAngle.value = Mathf.Lerp(originalIntensity, intensity, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                motionBlur.shutterAngle.value = Mathf.Lerp(intensity, originalIntensity, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            motionBlur.shutterAngle.value = originalIntensity;
        }
    }
}
