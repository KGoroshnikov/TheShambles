using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    public float rotationSpeed;
    public float moveSpeed;
    public float runSpeed;
    public float crouchedMoveSpeed;
    public float currentSpeed;
    public float jumpForce;

    public Rigidbody rb;
    public TipUI tipUI;

    public Animator animator;
    public string state;

    public float defaultAnimSpeed;
    public float runAnimSpeed;


    public Collider normalCollider;
    public Collider crouchedCollider;

    public bool isCrouched;
    public bool underZabor;
    public bool canJump = true;
    private bool holdingBox;
    public LayerMask lm;

    private void Start()
    {
        animator.Play("Idle");
        currentSpeed = moveSpeed;
    }

    public void boxTaken()
    {
        holdingBox = true;
    }

    public void AnimTake()
    {
        state = "walkItem";
        animator.speed = defaultAnimSpeed;
        animator.CrossFade("walkWithItem", 0.5f);
    }

    public void AnimUntake()
    {
        state = "walk";
        animator.speed = defaultAnimSpeed;
        animator.CrossFade("Walk", 0.5f);
    }

    public void boxDropped()
    {
        holdingBox = false;
    }

    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input = input.normalized;

        if (input == Vector3.zero)
        {
            if (state == "walk")
            {
                state = "idle";
                animator.speed = 1;
                animator.CrossFade("Idle", 0.5f);
            }
            else if (state == "walkItem")
            {
                animator.speed = 0.2f;
            }
        }


        if (input != Vector3.zero)
        {
            if (state == "idle")
            {
                state = "walk";
                animator.speed = defaultAnimSpeed;
                animator.CrossFade("Walk", 0.5f);
            }
            else if (state == "walkItem")
                animator.speed = defaultAnimSpeed;

            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;
            Vector3 moveDirection = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y + targetAngle, 0) * Vector3.forward;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 dirMove = transform.forward;
            rb.MovePosition(transform.position + dirMove * currentSpeed * Time.deltaTime);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f, lm))
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && !holdingBox && !underZabor)
        {
            if (state == "walk"){
                animator.speed = runAnimSpeed;
                currentSpeed = runSpeed;
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift) && !underZabor || holdingBox)
        {
            if (state == "walk") animator.speed = defaultAnimSpeed;
            if (state != "crouch") currentSpeed = moveSpeed;
        }

        if (Input.GetKeyUp(KeyCode.Space) && canJump && !underZabor && !holdingBox)
        {
            canJump = false;
            rb.velocity += Vector3.up * jumpForce + transform.forward * 2;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouched)
            {
                normalCollider.enabled = false;
                crouchedCollider.enabled = true;
                currentSpeed = crouchedMoveSpeed;
                animator.speed = 1;
                animator.CrossFade("crouch", 0.5f);
            }
            isCrouched = true;
            state = "crouch";
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
        {
            if (isCrouched && !underZabor)
            {
                normalCollider.enabled = true;
                crouchedCollider.enabled = false;
                currentSpeed = moveSpeed;
                state = "idle";
            }
            isCrouched = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TriggerCrouch")
        {
            tipUI.ShowTip("CTRL", "CROUCH");
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
            tipUI.HideTip();
            if (!isCrouched && currentSpeed == crouchedMoveSpeed)
            {
                normalCollider.enabled = true;
                crouchedCollider.enabled = false;
                currentSpeed = moveSpeed;
                state = "idle";
            }
            if (!other.transform.GetChild(0).gameObject.activeSelf)
                other.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
