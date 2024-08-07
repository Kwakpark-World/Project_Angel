using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionTrailEffect : MonoBehaviour
{
    public ParticleSystem trail;
    public Transform parentTransform; 

    void LateUpdate()
    {
        if (parentTransform != null)
        {
            trail.Play();
            //transform.position = parentTransform.position;
        }
    }
}
