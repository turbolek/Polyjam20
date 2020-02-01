using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDisplayer : MonoBehaviour
{
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 50);
    }
}
