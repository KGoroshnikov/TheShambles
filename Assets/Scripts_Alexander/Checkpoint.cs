using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
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

        if (checkPoints.Count - 1 == lastCheckpoint)
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

        Debug.Log(lastCheckpoint + " triggered!");


        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        GameData data = new GameData(allObjects.Length);
        for (int i = 0; i < allObjects.Length; i++)
        {
            GameObject go = allObjects[i];
            data.states[i] = new ObjectState(go.name, go.transform.position, go.transform.rotation);
        }
       
        File.WriteAllText("save.gd", JsonUtility.ToJson(data));
    }

    public static void LoadLastCheckpoint()
    {
        GameData data = JsonUtility.FromJson<GameData>(File.ReadAllText("save.gd"));
        
        foreach(ObjectState state in data.states)
        {
            GameObject go = GameObject.Find(state.name);
            go.transform.position = state.position;
            go.transform.rotation = state.rotation;

            EnemyDogAI ai = go.GetComponent<EnemyDogAI>();
            if (ai != null) ai.startChasing = false;
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
