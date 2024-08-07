using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionTrailEffect : MonoBehaviour
{
    public Transform parentTransform; 

    void LateUpdate()
    {
        if (parentTransform != null)
        {
            transform.position = parentTransform.position;
        }
    }
}
