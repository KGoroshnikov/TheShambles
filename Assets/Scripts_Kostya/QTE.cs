using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class QTE : MonoBehaviour
{
    public TipUI tipUI;
    public Camera cam;
    public Volume v;
    private ChromaticAberration chrAber;
    private float fovSave;
    public float targetFov;

    private Transform player;
    public Animator animator;
    public float k;

    private bool isActive;
    private bool plrAnim;
    private float t;
    private Vector3 startPos;
    private Vector3 targetPos;

    private bool played;

    private void Start()
    {
        VolumeProfile proflile = v.sharedProfile;
        proflile.TryGet(out chrAber);
        fovSave = cam.fieldOfView;
    }

    void PauseAnim()
    {
        //animator.speed = 0.1f;
        Time.timeScale = 0.1f;
        cam.fieldOfView = targetFov;
        chrAber.intensity.value = 1f;
        isActive = true;
        tipUI.ShowTip("SPACE", "");
        player.GetComponent<DogMovement>().enabled = false;
        Invoke("AutoResume", 2.5f);
    }

    void AutoResume()
    {
        Debug.Log("er");
        // player died
        Time.timeScale = 1f;
        player.GetComponent<DogMovement>().enabled = true;
        plrAnim = false;
        isActive = false;
        chrAber.intensity.value = 0.05f;
        cam.fieldOfView = fovSave;
        tipUI.HideTip();
    }

    private void Update()
    {
        if (plrAnim)
        {
            t += Time.deltaTime * 2;
            player.position = Vector3.Lerp(startPos, targetPos, t);
            if (t >= 1)
            {
                CancelInvoke("AutoResume");
                player.GetComponent<DogMovement>().enabled = true;
                plrAnim = false;
                isActive = false;
                chrAber.intensity.value = 0.05f;
                cam.fieldOfView = fovSave;
                tipUI.HideTip();
            }
        }

        if (!isActive) return;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Time.timeScale = 1f;
            plrAnim = true;
            startPos = player.position;
            targetPos = player.position + player.forward * 3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !played)
        {
            played = true;
            player = other.transform.parent;
            animator.enabled = true;
            Invoke("PauseAnim", animator.runtimeAnimatorController.animationClips[0].length / k);
        }
    }
}