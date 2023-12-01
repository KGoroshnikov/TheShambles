using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    public float rotationSpeed;
    public float moveSpeed;
    public float crouchedMoveSpeed;
    private float currentSpeed;
    public float jumpForce;
    public float jumpKD;

    public Rigidbody rb;


    public Collider normalCollider;
    public Collider crouchedCollider;

    public bool isCrouched;
    public bool underZabor;
    public bool canJump = true;

    private void Start()
    {
        currentSpeed = moveSpeed;
    }

    void ResetJump() => canJump = true;

    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input = input.normalized;

        if (input != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;
            Vector3 moveDirection = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y + targetAngle, 0) * Vector3.forward;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 dirMove = transform.forward;
            rb.MovePosition(transform.position + dirMove * currentSpeed * Time.deltaTime);
        }

        if (Input.GetKeyUp(KeyCode.Space) && canJump && !underZabor)
        {
            canJump = false;
            Invoke("ResetJump", jumpKD);
            //rb.AddForce(Vector3.up * jumpForce + transform.forward * 20, ForceMode.Impulse);
            rb.velocity += Vector3.up * jumpForce + transform.forward * 2;
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

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
        {
            if (isCrouched && !underZabor)
            {
                normalCollider.enabled = true;
                crouchedCollider.enabled = false;
                currentSpeed = moveSpeed;
            }
            isCrouched = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TriggerCrouch")
        {
            underZabor = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "TriggerCrouch")
        {
            if (isCrouched && other.transform.GetChild(0).gameObject.activeSelf)
                other.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "TriggerCrouch")
        {
            underZabor = false;
            if (!isCrouched && currentSpeed == crouchedMoveSpeed)
            {
                normalCollider.enabled = true;
                crouchedCollider.enabled = false;
                currentSpeed = moveSpeed;
            }
            if (!other.transform.GetChild(0).gameObject.activeSelf)
                other.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
