using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    public Transform[] allPoints;
    public NavMeshAgent agent;

    private bool chase;

    private void Start()
    {
        //playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        InvokeRepeating("GetTarget", 0, 6);
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    void GetTarget()
    {
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(allPoints[Random.Range(0, allPoints.Length)].position, out myNavHit, 100, -1))
        {
            agent.SetDestination(myNavHit.position);
        }
    }

    private void Update()
    {
        if (chase)
        {
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(playerRef.transform.position, out myNavHit, 100, -1))
            {
                agent.SetDestination(myNavHit.position);
            }
        }
    }

    void StopChase()
    {
        CancelInvoke("StopChase");
        chase = false;
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;

        if (canSeePlayer && !chase)
        {
            CancelInvoke("StopChase");
            chase = true;
        }
        else if (!canSeePlayer && chase)
        {
            Invoke("StopChase", 5);
        }
    }
}