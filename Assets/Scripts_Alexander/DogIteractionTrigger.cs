using UnityEngine;

public class DogIteractionTrigger : MonoBehaviour
{
    private Rigidbody rb;

    private void Update()
    {
        if (rb != null)
        {
            if (Input.GetMouseButton(0))
                rb.isKinematic = false;
            else rb.isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Movable")
            rb = collision.rigidbody;
    }
}
