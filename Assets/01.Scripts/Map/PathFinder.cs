using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class PathFinder : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public NavMeshAgent _navMeshAgent;

    public List<Transform> _targetList; // 시작을 0

    private int _targetIndex;

    private void Start()
    {
        
        InitNaviManager(0.01f);
    }

    public void InitNaviManager(float updateDelay)
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;

       _navMeshAgent = GetComponentInParent<NavMeshAgent>();

        _navMeshAgent.isStopped = true;
        _navMeshAgent.radius = 1;
        _navMeshAgent.height = 1;

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
                _navMeshAgent.SetDestination(_targetList[_targetIndex].position);
                DrawPath();
            }

            yield return delayTime;
        }
    }

    public void PassToNextDestination()
    {
        _targetIndex++;
    }

    private void DrawPath()
    {
        if (_navMeshAgent.path.corners.Length < 2)
        {
            _lineRenderer.positionCount = 0;
            return;
        }

        _lineRenderer.positionCount = _navMeshAgent.path.corners.Length;
        _lineRenderer.SetPosition(0, transform.position);

        for (int i = 1; i < _navMeshAgent.path.corners.Length; ++i)
        {
            _lineRenderer.SetPosition(i, _navMeshAgent.path.corners[i]);
        }
    }
}
