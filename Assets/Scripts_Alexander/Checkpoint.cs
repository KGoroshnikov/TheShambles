using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
            SaveState();

        checkPoints.Add(this);

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (checkpointGained) return;
        if (other.tag != "Player") return;
        SaveState();
    }

    private void SaveState()
    {
        Debug.Log("Triggered!");
        lastCheckpoint = checkPoints.IndexOf(this);
        checkpointGained = true;

        Dictionary<string, ObjectState> states = new Dictionary<string, ObjectState>();

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
            states.Add(go.name, new ObjectState(go.transform.position, go.transform.rotation));
        
        File.WriteAllText("save.gd", JsonUtility.ToJson(states));
    }

    public static void LoadLastCheckpoint()
    {
        Dictionary<string, ObjectState> states = JsonUtility.FromJson<Dictionary<string, ObjectState>>(File.ReadAllText("save.gd"));

        foreach(string name in states.Keys)
        {
            Transform transform = GameObject.Find(name).transform;
            transform.position = states[name].position;
            transform.rotation = states[name].rotation;
        }
    }

    [System.Serializable]
    private class ObjectState
    {
        public Vector3 position;
        public Quaternion rotation;

        public ObjectState(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}
