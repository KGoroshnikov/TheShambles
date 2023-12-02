using Unity.Mathematics;
using UnityEngine;

public class DogIteractionTrigger : MonoBehaviour
{
    private Rigidbody rb;

    private GameObject box;
    public DogMovement dogMovement;
    public CameraFollow cameraFollow;

    public float distHold;

    private bool taken;

    private void Update()
    {
        if (box != null)
        {
            Vector3 pos = Vector3.zero;
            if (Input.GetMouseButton(0))
            {
                if (!taken)
                {
                    pos = transform.position + transform.forward * distHold;
                    pos.y = box.transform.position.y;
                    box.transform.position = pos;
                    rb = box.GetComponent<Rigidbody>();
                    dogMovement.boxTaken();
                    taken = true;
                }

                pos = transform.position + transform.forward * distHold;
                pos.y = box.transform.position.y;
                if (Vector3.Distance(pos, new Vector3(transform.position.x, pos.y, transform.position.z)) < 0.5f)
                {
                    box = null;
                    rb = null;
                    dogMovement.boxDropped();
                    taken = false;
                    return;
                }

                rb.MovePosition(box.transform.position + (-box.transform.position + pos) * Time.deltaTime * 100);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (taken)
                {
                    box = null;
                    rb = null;
                    dogMovement.boxDropped();
                    taken = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Movable")
        {
            box = collision.gameObject;
            //cameraFollow.boxTaken();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Movable" && !taken)
        {
            box = null;
            //cameraFollow.boxTaken();
        }
    }
}
