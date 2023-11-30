using System;
using System.Collections.Generic;
using System.Data;
using UnityEditor.SearchService;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static List<Checkpoint> checkPoints = new List<Checkpoint>();
    private static int lastCheckpoint = 0;

    public bool checkpointGained;


    private void Start()
    {
        Debug.Log(transform.position + " - " + checkPoints.Count);

        if (checkPoints.Count == lastCheckpoint) 
            checkpointGained = true;

        checkPoints.Add(this);

        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (checkpointGained) return;
        Debug.Log("Triggered!");
        lastCheckpoint = checkPoints.IndexOf(this);
        checkpointGained = true;
    }

    public static void LoadLastCheckpoint(MonoBehaviour obj)
    {
        obj.transform.position = checkPoints[lastCheckpoint].transform.position;
        // TODO: Return game state checkpoint
    }
}
