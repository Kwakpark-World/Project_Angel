using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathFinder : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    public List<Transform> _targetList; // 시작을 0
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
                _lineRenderer.enabled = false;
                break;
            }

            if (Vector3.Distance(transform.position, _targetList[_targetIndex].position) <= 2f)
            {
                PassToNextDestination();
            }

            if (_targetIndex < _targetList.Count)
            {
                DrawPathToCurrentTarget();
            }

            yield return delayTime;
        }
    }

    public void PassToNextDestination()
    {
        _targetIndex++;
    }

    private void DrawPathToCurrentTarget()
    {
        if(_mode == BGMMode.NonCombat)
        {
            if (_targetIndex < _targetList.Count)
            {
                Vector3[] pathCorners = new Vector3[2];
                pathCorners[0] = transform.position;
                pathCorners[1] = _targetList[_targetIndex].position;

                _lineRenderer.positionCount = pathCorners.Length;
                _lineRenderer.SetPositions(pathCorners);
            }
        }
 
    }
}
