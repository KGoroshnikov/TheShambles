using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform camHelper;

    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public Vector3 offsetLook;
    public float radius;
    public float sens;
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    public float currentAngle;

    private void Update()
    { 
        float mouseX = Input.GetAxis("Mouse X") * sens;
        currentAngle += mouseX;
        Vector3 p = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle)) * radius;
        transform.LookAt(target.position + offsetLook);

        Vector3 targetPosition = target.position + offset + p;
        targetPosition.y = transform.position.y;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
