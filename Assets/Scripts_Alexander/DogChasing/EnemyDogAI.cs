using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDogAI : MonoBehaviour
{
    public Transform goal;
    public bool startChasing = false;
    
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!startChasing)
        {
            agent.destination = transform.parent.position;
            agent.stoppingDistance = 5;
            return;
        }
        agent.destination = goal.position;
        agent.stoppingDistance = 0;
    }

    public void DisableDogs()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!startChasing) return;
        if (collision.gameObject.tag != "Player") return;
        Checkpoint.LoadLastCheckpoint();
    }

    public NavMeshAgent GetNavMeshAgent() => agent;
}
