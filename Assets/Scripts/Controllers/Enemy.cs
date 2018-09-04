using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Include Artificial Intelligence part of API
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Seek
    }
    // Property
    public int Health
    {
        get
        {
            return health;
        }
    }
    public State currentState = State.Patrol;
    public NavMeshAgent agent;

    public FieldOfView fov;
    public AudioSource alertSound;
    public GameObject alertSymbol;

    public Transform target;
    public Transform waypointParent;
    public float distanceToWaypoint = 1f;
    public float detectionRadius = 5f;
    private int health = 100;
    private int currentIndex = 1;
    private Transform[] waypoints;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    void Start()
    {
        waypoints = waypointParent.GetComponentsInChildren<Transform>();
    }
    void Patrol()
    {
        alertSymbol.SetActive(false);

        if (currentIndex >= waypoints.Length)
        {
            currentIndex = 1;
        }

        Transform point = waypoints[currentIndex];

        float distance = Vector3.Distance(transform.position, point.position);
        if (distance <= distanceToWaypoint)
        {
            currentIndex++;
        }

        agent.SetDestination(point.position);

        if(fov.visibleTargets.Count > 0)
        {
            target = fov.visibleTargets[0];
            currentState = State.Seek;
            alertSound.Play();
            alertSymbol.SetActive(false);
        }
    }
    void Seek()
    {
        // Update the AI's target position
        agent.SetDestination(target.position);

        // Get distance to target
        float distToTarget = Vector3.Distance(transform.position, target.position);
        // If the target is within detection range
        if (distToTarget >= detectionRadius)
        {
            // Switch to 'Seek' state
            currentState = State.Patrol;
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Seek:
                Seek();
                break;
            default:
                break;
        }
    }

    //void FixedUpdate()
    //{
    //    Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
    //    foreach (var hit in hits)
    //    {
    //        Player player = hit.GetComponent<Player>();
    //        if (player)
    //        {
    //            target = player.transform;
    //            return;
    //        }
    //    }

    //    target = null;
    //}
    public void DealDamage(int damageDealt)
    {
        health -= damageDealt;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}