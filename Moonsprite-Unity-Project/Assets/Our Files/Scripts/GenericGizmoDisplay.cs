using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGizmoDisplay : MonoBehaviour
{
    public enum GizmoShape
    {
        Square,
        Circle
    }

    public bool active = true;
    [Space]
    public float size = 0.1f;
    public GizmoShape shape = GizmoShape.Square;

    void OnDrawGizmos()
    {
        if(active == false)
        {
            return;
        }

        switch (shape)
        {
            case GizmoShape.Square:
                Gizmos.DrawWireCube(transform.position, new Vector3(size, size));
                break;
            case GizmoShape.Circle:
                Gizmos.DrawWireSphere(transform.position, size);
                break;
        }
    }
}
