using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor.SearchService;
using UnityEditorInternal;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static List<Checkpoint> checkPoints = new List<Checkpoint>();
    private static int lastCheckpoint = 0;

    public bool checkpointGained;


    private void Start()
    {
        Debug.Log(transform.position + " - " + checkPoints.Count);

        checkPoints.Add(this);

        if (checkPoints.Count == lastCheckpoint)
            SaveState();


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
        lastCheckpoint = checkPoints.IndexOf(this);
        checkpointGained = true;

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        GameData data = new GameData(allObjects.Length);
        for (int i = 0; i < allObjects.Length; i++)
            data.states[i] = new ObjectState(
                allObjects[i].name,
                allObjects[i].transform.position,
                allObjects[i].transform.rotation);
       
        File.WriteAllText("save.gd", JsonUtility.ToJson(data));

        Debug.Log(lastCheckpoint + " triggered!");
    }

    public static void LoadLastCheckpoint()
    {
        GameData data = JsonUtility.FromJson<GameData>(File.ReadAllText("save.gd"));

        foreach(ObjectState state in data.states)
        {
            Transform transform = GameObject.Find(state.name).transform;
            transform.position = state.position;
            transform.rotation = state.rotation;
        }
    }

    [System.Serializable]
    private class GameData
    {
        public ObjectState[] states;

        public GameData(int stateCount)
        {
            states = new ObjectState[stateCount];
        }
    }

    [System.Serializable]
    private class ObjectState
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;

        public ObjectState(string name, Vector3 position, Quaternion rotation)
        {
            this.name = name;
            this.position = position;
            this.rotation = rotation;
        }
    }
}
