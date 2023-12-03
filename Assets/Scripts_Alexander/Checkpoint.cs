using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private static List<Checkpoint> checkPoints = new List<Checkpoint>();
    private static int lastCheckpoint = 0;

    public string playerObjectName;

    public bool checkpointGained;

    public UnityEvent afterGained;


    private void Start()
    {
        Debug.Log(transform.position + " - " + checkPoints.Count);

        checkPoints.Add(this);

        if (checkPoints.Count - 1 == lastCheckpoint)
        {
            SaveState();
            Invoke("MovePlayer", 0);
        }

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }
    private void MovePlayer()
    {
        lastCheckpoint = PlayerPrefs.GetInt("Last Checkpoint", 0);

        GameObject.Find(playerObjectName).transform.position = checkPoints[lastCheckpoint].transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (checkpointGained) return;
        if (other.tag != "Player") return;
        SaveState();
        afterGained.Invoke();
    }

    private void SaveState()
    {
        lastCheckpoint = checkPoints.IndexOf(this);
        checkpointGained = true;
        PlayerPrefs.SetInt("Last Checkpoint", lastCheckpoint);

        Debug.Log(lastCheckpoint + " triggered!");
    }

    public static void LoadLastCheckpoint()
    {
        checkPoints.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
