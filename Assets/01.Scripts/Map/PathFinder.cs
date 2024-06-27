using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathFinder : MonoBehaviour
{
    public LineRenderer PathLine { get; private set; }

    public List<Transform> targetList;
    private int _targetIndex;

    private void Start()
    {
        InitNaviManager(0.01f);
    }

    public void InitNaviManager(float updateDelay)
    {
        PathLine = GetComponent<LineRenderer>();
        PathLine.positionCount = 0;

        StartCoroutine(UpdateNavi(updateDelay));
    }

    IEnumerator UpdateNavi(float delay)
    {
        WaitForSeconds delayTime = new WaitForSeconds(delay);

        while (true)
        {
            if (_targetIndex >= targetList.Count)
            {
                PathLine.enabled = false;

                break;
            }

            if (!PathLine.enabled)
            {
                continue;
            }

            if (Vector3.Distance(transform.position, targetList[_targetIndex].position) <= 5f)
            {
                PassToNextDestination();
            }

            if (_targetIndex < targetList.Count)
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
        if (_targetIndex < targetList.Count)
        {
            Vector3[] pathCorners = new Vector3[2];
            pathCorners[0] = transform.position;
            pathCorners[1] = targetList[_targetIndex].position;
            PathLine.positionCount = pathCorners.Length;
            PathLine.SetPositions(pathCorners);
            PathLine.enabled = true; 
        }
    }
}
