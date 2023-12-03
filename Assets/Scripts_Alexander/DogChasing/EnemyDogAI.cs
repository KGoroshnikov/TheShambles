using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyDogAI : MonoBehaviour
{
    public Transform goal;
    public bool startChasing = false;
    
    private NavMeshAgent agent;

    public Animator animator;
    private string state = "Idle";

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator.Play("Idle", -1, Random.value);
        agent.stoppingDistance = 0;
    }

    public void StartChase()
    {
        agent.destination = goal.position;
        state = "Chase";
        startChasing = true;
        animator.speed = 2.2f;
        animator.Play("walkSave", -1, Random.value);
    }

    public void StopChase()
    {
        startChasing = false;
    }

    private void Update()
    {
        if (!agent.pathPending && state != "Idle")
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    animator.speed = 1f;
                    animator.CrossFade("Idle", 0.5f);
                    state = "Idle";
                }
            }
        }

        if (!startChasing)
        {
            return;
        }
        agent.destination = goal.position;
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
