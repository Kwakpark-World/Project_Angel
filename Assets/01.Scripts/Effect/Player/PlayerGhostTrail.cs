using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGhostTrail : MonoBehaviour
{
    public float activeTime = 2f;

    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;

    // root Position
    public Transform positionToSpawn;

    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    private bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private void Update()
    {
#if UNITY_EDITOR // Debug
        if (Keyboard.current.spaceKey.wasPressedThisFrame)//&& !isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(GhostTrail(activeTime));
        }
#endif
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

                Vector3 position = positionToSpawn.position;
                Quaternion rotation = positionToSpawn.rotation;
                
                gObj.transform.SetPositionAndRotation(position, rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                mf.mesh = mesh;
                mr.material = mat;

                StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDestroyDelay); // Change Pool
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }
    }

    private IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
