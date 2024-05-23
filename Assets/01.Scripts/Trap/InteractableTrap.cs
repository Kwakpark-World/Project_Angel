using AmplifyShaderEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableTrap : Trap
{
    protected bool _isDelay;

    public void StartDelayAction(float delayTime, Action todoAction)
    {
        StartCoroutine(DelayAction(delayTime, todoAction));
    }

    public void StartDelayAction(Action todoAction)
    {
        _isDelay = true;
        StartCoroutine(DelayAction(todoAction));
    }

    protected IEnumerator DelayAction(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    protected IEnumerator DelayAction(Action action)
    {
        while (_isDelay)
        {
            yield return null;
        }
        action?.Invoke();
    }
}
