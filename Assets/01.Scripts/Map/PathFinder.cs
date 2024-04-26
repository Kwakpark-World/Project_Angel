using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(NavMeshAgent))]
public class PathFinder : MonoBehaviour
{
    private LineRenderer _lineRender;
    private NavMeshAgent _navMashAgent;

    public List<Vector3> _targetList; //Ω√¿€¿ª 0

    private int _targetIndex;

    private void Start()
    {
        InitNaviManager(0.01f);
    }

    public void InitNaviManager(float updateDelay)
    {
        //SetOriginTransform(trans);

        _lineRender = GetComponent<LineRenderer>();
        _lineRender.startWidth = 0.5f;
        _lineRender.endWidth = 0.5f;
        _lineRender.positionCount = 0;

        _navMashAgent = GetComponent<NavMeshAgent>();
        _navMashAgent.isStopped = true;
        _navMashAgent.radius = 1;
        _navMashAgent.height = 1;

        StartCoroutine(UpdateNavi(updateDelay));
    }

    IEnumerator UpdateNavi(float delay)
    {
        WaitForSeconds delayTime = new WaitForSeconds(delay);

        while (true)
        {
            if (Vector3.Distance(transform.position, _targetList[_targetIndex]) <= 2f)
            {
                PassToNextDestination();
            }

            if (_targetIndex >= _targetList.Count)
            {
                _lineRender.enabled = false;

                break;
            }

            transform.position = GameManager.Instance.PlayerInstance.transform.position;
            _navMashAgent.SetDestination(_targetList[_targetIndex]);
            DrawPath();
            yield return delayTime;
        }
    }

    public void PassToNextDestination()
    {
        _targetIndex++;
    }

    private void DrawPath()
    {
        _lineRender.positionCount = _navMashAgent.path.corners.Length;
        _lineRender.SetPosition(0, transform.position);

        if (_navMashAgent.path.corners.Length < 2)
        {
            return;
        }

        for (int i = 1; i < _navMashAgent.path.corners.Length; ++i)
        {
            _lineRender.SetPosition(i, _navMashAgent.path.corners[i]);
        }
    }
}
