using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DogIteractionTrigger : MonoBehaviour
{
    public float grabSpeedMultiplier = 0.5f;

    void Start()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Input.GetMouseButton(0))
        {
            if (collision.gameObject.tag == "Movable")
            {
                Rigidbody rb = collision.rigidbody;


            }
        }
    }
}
