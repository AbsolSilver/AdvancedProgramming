using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int Health
    {
        get
        {
            return health;
        }
    }

    public NavMeshAgent agent;
    public Transform target;
    public Transform waypointParent;
    public bool loop = false;
    public float distanceToWaypoint = 1f;

    private Transform[] waypoints;
    private bool pingPong = false;
    private int currentIndex = 1;
    private int health = 100;
    
    void Start()

    {
        waypoints = waypointParent.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            // Update the AI's target position
            agent.SetDestination(target.position);
        }
        else
        {
            // If the currentIndex exceeds size of array
            if (currentIndex >= waypoints.Length)
            {
                if (loop)
                {
                    // Reset back to "first" waypoint
                    currentIndex = 1;
                }
                else
                {
                    currentIndex = waypoints.Length - 1;
                    pingPong = true;
                }
            }

            // If the currentIndex goes below 0
            if (currentIndex <= 0)
            {
                if (loop)
                {
                    currentIndex = waypoints.Length - 1;
                }
                else
                {
                    // Reset back to "first" waypoint
                    currentIndex = 1;
                    
                    pingPong = false;
                }

            }

            Transform point = waypoints[currentIndex];
            float distance = Vector3.Distance(transform.position, point.position);

            if (distance <= distanceToWaypoint)
            {
                if (pingPong)
                {
                    currentIndex--;
                }
                else
                {
                    currentIndex++;
                }

            }

            agent.SetDestination(point.position);
        }
    }

    public void DealDamage(int damageDealt)
    {
        health -= damageDealt;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
