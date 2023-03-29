using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisProjectileScript : MonoBehaviour
{
    public GameObject dustCloudPrefab;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Squatter")
        {
            CollisionWithSquatter(collision);
        }
    }

    void CollisionWithSquatter(Collider2D collision)
    {
        SquatterScript squatterScript = collision.transform.GetComponent<SquatterScript>();
        if(squatterScript != null)
        {
            // this shouldn't be needed (as we shouldn't have to check if it is null or not), but it is for some reason
            squatterScript.DebrisCollision();
        }

        Instantiate(dustCloudPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
