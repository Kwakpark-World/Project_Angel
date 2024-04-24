using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinder : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> targets;
    public LineRenderer playerLineRenderer; // Player�� LineRenderer
    // public List<LineRenderer> targetLineRenderers; // �� ������Ʈ�� LineRenderer (����)

    public float distanceThreshold = 1f; // �÷��̾ Ÿ�ٿ� �����ϴ� �Ÿ� �Ӱ谪

    // Start is called before the first frame update
    void Start()
    {
        // Player�� LineRenderer �ʱ�ȭ
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

        // �÷��̾��� LineRenderer ����
        playerLineRenderer.positionCount = targets.Count + 1;
        playerLineRenderer.SetPosition(0, transform.position);

        // ������Ʈ���� LineRenderer ����
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                // �÷��̾�� Ÿ�� ������ LineRenderer�� �׸���
                // playerLineRenderer.SetPosition(i + 1, targets[i].transform.position); // �� �� �ּ� ó��

                // �÷��̾ Ÿ�ٿ� ������ �ִ��� Ȯ��
                if (Vector3.Distance(player.transform.position, targets[i].transform.position) < distanceThreshold)
                {
                    // Ÿ���� LineRenderer ��Ȱ��ȭ
                    // targetLineRenderers[i].enabled = false; // �� �� �ּ� ó��
                    Debug.Log("3");
                }
                else
                {
                    Debug.Log("999");
                    // Ÿ���� LineRenderer Ȱ��ȭ
                    // targetLineRenderers[i].enabled = true; // �� �� �ּ� ó��
                }
            }
        }
    }
}
