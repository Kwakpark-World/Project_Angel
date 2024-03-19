using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEffect : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float rotateAmount;

    private void Update()
    {
        var dir = Vector3.forward + Vector3.right * rotateAmount;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        float rotationAmount =  rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, rotationAmount);
    }
}
