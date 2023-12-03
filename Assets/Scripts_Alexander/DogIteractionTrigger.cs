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

    private bool isFood;

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
                    taken = true;

                    if (box.name == "FOOD")
                    {
                        isFood = true;
                        rb.isKinematic = true;
                    }
                    else dogMovement.boxTaken();
                    dogMovement.AnimTake();
                }

                pos = transform.position + transform.forward * distHold;
                if (!isFood) pos.y = box.transform.position.y;
                if (Vector3.Distance(pos, new Vector3(transform.position.x, pos.y, transform.position.z)) < 0.5f && !isFood)
                {
                    box = null;
                    rb = null;
                    dogMovement.boxDropped();
                    dogMovement.AnimUntake();
                    taken = false;
                    isFood = false;
                    return;
                }

                if (!isFood)
                    rb.MovePosition(box.transform.position + (-box.transform.position + pos) * Time.deltaTime * 100);
                else
                    box.transform.position = Vector3.MoveTowards(box.transform.position, pos, Time.deltaTime * 100);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (taken)
                {
                    if (isFood) rb.isKinematic = false;
                    dogMovement.AnimUntake();
                    box = null;
                    rb = null;
                    dogMovement.boxDropped();
                    taken = false;
                    isFood = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Movable" && !taken)
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
