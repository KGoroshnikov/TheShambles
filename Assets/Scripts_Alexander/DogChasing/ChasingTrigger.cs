using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ChasingTrigger : MonoBehaviour
{
    public EnemyDogAI[] dogs;

    void Start()
    {

        GameObject root = GameObject.Find("ChasingDogs");
        if (root == null) return;
        dogs = new EnemyDogAI[root.transform.childCount];
        for (int i = 0; i < root.transform.childCount; i++)
            dogs[i] = root.transform.GetChild(i).GetComponent<EnemyDogAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        for (int i = 0; i < dogs.Length; i++)
        {
            dogs[i].GetComponent<NavMeshAgent>().enabled = true;
            dogs[i].startChasing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        for (int i = 0; i < dogs.Length; i++)
        {
            dogs[i].startChasing = false;
            dogs[i].GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}
