using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathFinder : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    public List<Transform> _targetList;
    private int _targetIndex;

    private BGMMode _mode;

    private void Start()
    {
        InitNaviManager(0.01f);
    }

    public void InitNaviManager(float updateDelay)
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;

        StartCoroutine(UpdateNavi(updateDelay));
    }

    IEnumerator UpdateNavi(float delay)
    {
        WaitForSeconds delayTime = new WaitForSeconds(delay);

        while (true)
        {
            if (_targetIndex >= _targetList.Count)
            {
                Debug.Log("No more targets.");
                _lineRenderer.enabled = false;
                break;
            }

            if (Vector3.Distance(transform.position, _targetList[_targetIndex].position) <= 2f)
            {
                Debug.Log("Reached target " + _targetIndex);
                PassToNextDestination();
            }

            if (_targetIndex < _targetList.Count)
            {
                Debug.Log(UIManager.Instance._mode);
                if (UIManager.Instance._mode == BGMMode.Combat)
                {
                    Debug.Log("Combat mode: Clearing path.");
                    _lineRenderer.positionCount = 0;
                }
                else if (UIManager.Instance._mode == BGMMode.NonCombat)
                {
                    Debug.Log("NonCombat mode: Drawing path.");
                    DrawPathToCurrentTarget();
                }
            }

            yield return delayTime;
        }
    }

    public void PassToNextDestination()
    {
        _targetIndex++;
        Debug.Log("Passed to next target: " + _targetIndex);
    }

    private void DrawPathToCurrentTarget()
    {
        Debug.Log("Inside DrawPathToCurrentTarget");
        if (_targetIndex < _targetList.Count)
        {
            Debug.Log("Drawing path to target " + _targetIndex);
            Vector3[] pathCorners = new Vector3[2];
            pathCorners[0] = transform.position;
            pathCorners[1] = _targetList[_targetIndex].position;
            _lineRenderer.positionCount = pathCorners.Length;
            _lineRenderer.SetPositions(pathCorners);
            _lineRenderer.enabled = true;  // Ensure the LineRenderer is enabled
            Debug.Log("Path drawn to target " + _targetIndex);
        }
        else
        {
            Debug.Log("Target index out of bounds: " + _targetIndex);
        }
    }

    public void SetMode(BGMMode newMode)
    {
        _mode = newMode;
        Debug.Log("Mode set to " + _mode);
    }
}
