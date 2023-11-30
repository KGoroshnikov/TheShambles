using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    public float rotationSpeed;
    public float moveSpeed;
    public float crouchedMoveSpeed;
    private float currentSpeed;

    public Rigidbody rb;


    public Collider normalCollider;
    public Collider crouchedCollider;

    public bool isCrouched;

    private void Start()
    {
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input = input.normalized;

        if (input != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 dirMove = transform.forward;
            rb.MovePosition(transform.position + dirMove * currentSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouched)
            {
                normalCollider.enabled = false;
                crouchedCollider.enabled = true;
                currentSpeed = crouchedMoveSpeed;
            }
            isCrouched = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouched)
            {
                normalCollider.enabled = true;
                crouchedCollider.enabled = false;
                currentSpeed = moveSpeed;
            }
            isCrouched = false;
        }

    }
}
