using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMover : MonoBehaviour, IMover
{
    float IMover.MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }
    [SerializeField] float moveSpeed = 1;

    // Must be called on fixed update to work correctly
    void IMover.Move(Vector2 moveVector)
    {
        Vector3 newPosition = transform.position + (new Vector3(moveVector.x, moveVector.y, 0) * moveSpeed);
        transform.position = newPosition;
    }
}
