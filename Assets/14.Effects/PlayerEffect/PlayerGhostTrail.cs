using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhostTrail : MonoBehaviour
{
    public float activeTime = 2f;

    public float meshRefreshRate = 0.1f;
    public Transform D;

    private bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(GhostTrail(activeTime));
        }
    }

    private IEnumerator GhostTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i <  skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                //gObj.transform.SetPositionAndRotation();

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }
    }
}
