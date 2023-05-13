using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGameObjectAlongVector : MonoBehaviour
{
    public Vector3 movementVector = Vector3.zero;

    void FixedUpdate()
    {
        transform.position = transform.position + movementVector;
    }
}
