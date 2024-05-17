using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazierCurveEffect : MonoBehaviour
{
    [SerializeField] private Transform[] _trms;
    [SerializeField] private float _speed;

    [ContextMenu("Bazier")]
    public void StartMove()
    {
        StartCoroutine(MovementCo());
    }

    private IEnumerator MovementCo()
    {
        float dt;
        float percent = 0.0f;
        Vector3 p1;
        Vector3 p2;
        Vector3 result;
        transform.position = Vector3.zero;
        while(Vector3.Distance(transform.position, _trms[2].position) >= 0.1f)
        {
            dt = Time.deltaTime * _speed;
            percent += dt;
            p1 = Vector3.Lerp(_trms[0].position, _trms[1].position, percent);
            p2 = Vector3.Lerp(_trms[1].position, _trms[2].position, percent);
            result = Vector3.Lerp(p1, p2, percent);

            transform.position = result;

            yield return null;
        }
    }
}
