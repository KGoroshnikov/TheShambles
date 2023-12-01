using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InteractK : MonoBehaviour
{
    private Transform doorObj;
    public float progressGain;
    public float progressLoose;
    public Rigidbody rb;

    public float dogLength;
    private Transform handlePoint;
    private Vector3 direct;

    public bool canInteract;
    public bool unlocked;
    public float progress;
    public bool interacting;

    private void Update()
    {
        if ( (!canInteract || unlocked) && !interacting) return;

        if (interacting)
        {
            float aw = math.pow(dogLength, 2) - math.pow(handlePoint.position.y, 2);
            Vector3 pp = handlePoint.position + direct * math.sqrt(aw);
            pp.y = transform.position.y;
            transform.position = pp;
            transform.LookAt(handlePoint.position);
        }

        progress -= Time.deltaTime * progressLoose;
        if (progress < 0)
        {
            progress = 0;
            if (interacting)
            {
                interacting = false;
                rb.isKinematic = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            progress += progressGain;
            interacting = true;
            rb.isKinematic = true;
        }

        if (progress >= 1)
        {
            interacting = false;
            unlocked = true;
            rb.isKinematic = false;
        }

        doorObj.transform.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-90, 0, 0), progress);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "openableObject" && !interacting)
        {
            doorObj = null;
            handlePoint = null;
            canInteract = false;
            progress = 0;
            unlocked = false;

            doorObj = other.transform.GetChild(0);
            canInteract = true;
            direct = -other.transform.parent.forward;
            handlePoint = other.transform.GetChild(0).Find("Handle");
            if (doorObj.transform.localEulerAngles.x != 0) unlocked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "openableObject" && !interacting)
        {
            doorObj = null;
            handlePoint = null;
            canInteract = false;
            progress = 0;
            unlocked = false;
        }
    }
}
