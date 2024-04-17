using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTestPlayer : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            RaycastHit[] a = Physics.RaycastAll(transform.position, transform.forward, 50f);
            a = Physics.RaycastAll(transform.position, -transform.forward, 50f);

            foreach(var b in a)
            {
                Debug.Log(b.transform.gameObject);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 50f);
        Gizmos.DrawRay(transform.position, -transform.forward * 50f);
    }
}
