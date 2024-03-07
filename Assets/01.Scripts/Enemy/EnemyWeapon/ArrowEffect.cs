using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEffect : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    public float spinSpeed;
    public float spinRadius;

    float value;

    private void Update()
    {
        value += Time.deltaTime * spinSpeed;

        float spinX = Mathf.Sin(value) * spinRadius;
        float spinY = Mathf.Cos(value) * spinRadius;


        var dir = Vector3.forward + Vector3.right;


        //Forward movement
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        float rotationAmount =  rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, rotationAmount);
    }
}
