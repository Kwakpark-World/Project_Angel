using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinder : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> targets;
    public LineRenderer playerLineRenderer; // Player의 LineRenderer
    // public List<LineRenderer> targetLineRenderers; // 각 오브젝트의 LineRenderer (제거)

    public float distanceThreshold = 1f; // 플레이어가 타겟에 도달하는 거리 임계값

    // Start is called before the first frame update
    void Start()
    {
        // Player의 LineRenderer 초기화
        playerLineRenderer.startWidth = playerLineRenderer.endWidth = 0.5f;
        playerLineRenderer.material.color = Color.blue;
        playerLineRenderer.enabled = false;
    }

    private void Update()
    {
        DrawPath();
    }

    public void DrawPath()
    {
        playerLineRenderer.enabled = true;

        // 플레이어의 LineRenderer 설정
        playerLineRenderer.positionCount = targets.Count + 1;
        playerLineRenderer.SetPosition(0, transform.position);

        // 오브젝트들의 LineRenderer 설정
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                // 플레이어와 타겟 사이의 LineRenderer만 그리기
                // playerLineRenderer.SetPosition(i + 1, targets[i].transform.position); // 이 줄 주석 처리

                // 플레이어가 타겟에 가까이 있는지 확인
                if (Vector3.Distance(player.transform.position, targets[i].transform.position) < distanceThreshold)
                {
                    // 타겟의 LineRenderer 비활성화
                    // targetLineRenderers[i].enabled = false; // 이 줄 주석 처리
                    Debug.Log("3");
                }
                else
                {
                    Debug.Log("999");
                    // 타겟의 LineRenderer 활성화
                    // targetLineRenderers[i].enabled = true; // 이 줄 주석 처리
                }
            }
        }
    }
}
