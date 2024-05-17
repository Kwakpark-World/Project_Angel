using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    private Coroutine _timeScaleChangeCoroutine;

    public float scale;
    public float tick;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            TimeChange(scale, tick);    
        }
        Debug.Log(Time.timeScale);
    }

    public void TimeChange(float targetScale, float tick)
    {
        ResetTimeChangeCoroutine();

        if (Time.timeScale < targetScale)
        {
            _timeScaleChangeCoroutine = StartCoroutine(TimeFast(targetScale, tick));
        }
        else
            _timeScaleChangeCoroutine = StartCoroutine(TimeSlow(targetScale, tick));
    }

    private void ResetTimeChangeCoroutine()
    {
        float prevTimeScale = Time.timeScale;

        if (_timeScaleChangeCoroutine != null)
        {
            StopCoroutine(_timeScaleChangeCoroutine);
            _timeScaleChangeCoroutine = null;

            Time.timeScale = prevTimeScale;
        }
    }

    private IEnumerator TimeSlow(float targetScale, float tick)
    {
        float tickScale = (Time.timeScale - targetScale) / tick;

        while (Time.timeScale > targetScale)
        {
            if (Time.timeScale - tickScale <= 0f)
            {
                Time.timeScale = 0f;
                break;
            }
            Time.timeScale -= tickScale;

            yield return new WaitForSecondsRealtime(1 / tick);
        }
    }

    private IEnumerator TimeFast(float targetScale, float tick)
    {
        float tickScale = (targetScale - Time.timeScale) / tick;

        while (Time.timeScale < targetScale)
        {
            Time.timeScale += tickScale;

            yield return new WaitForSecondsRealtime(1 / tick);
        }

        if (Time.timeScale > targetScale)
            Time.timeScale = targetScale;
    }
}
