using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public bool jumpBack;

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime);
        if (jumpBack) Checkpoint.LoadLastCheckpoint(this);
    }
}
