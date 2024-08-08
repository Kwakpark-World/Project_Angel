using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        AlignThis();
    }

    private void AlignThis()
    {
        if (Camera.main != null)
        {
            var camXform = Camera.main.transform;
            var forward = transform.position - camXform.position;

            forward.Normalize();

            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }
}
