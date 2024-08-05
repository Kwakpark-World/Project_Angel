using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMoveSphere : MonoBehaviour
{
    [SerializeField] private float _rad = 2.0f;
    [SerializeField] private float _speed = 2.0f;
    void Update()
    {
        transform.position = new Vector3
            (
                Mathf.Cos(Time.time * _speed) * _rad,
                0,
                Mathf.Sin(Time.time * _speed) * _rad
            );
    }
}
