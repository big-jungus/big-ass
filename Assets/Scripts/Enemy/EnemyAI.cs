using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sprite;
    
    [SerializeField] private CircleCollider2D detectionCollider;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Seeker seeker;

    private Transform target;
    private Path currentPath;
    private int currentWaypoint = 0;

    [Header("Movement Variables")]
    [SerializeField] private float speed;
    [SerializeField] private float nextWaypointDistance;

    [SerializeField] private MovementTypes movement;

    [SerializeField] private List<Vector2> pathNodes = new List<Vector2>();
    private int pathCounter;

    private void Start()
    {
        switch (movement)
        {
            case MovementTypes.FollowPlayer:
                InvokeRepeating("FollowPlayer", 0f, 0.5f);
                break;
            case MovementTypes.FollowPath:
                FollowNodes();
                break;
        }
        
    }

    private void FollowPlayer()
    {
        if (seeker.IsDone())
        {
            if (target != null)
                seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void FollowNodes()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.parent.position, pathNodes[pathCounter], OnFollowPathComplete);
        }
    }

    private void OnFollowPathComplete(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
        }
    }


    private void FixedUpdate()
    {
        if (currentPath == null)
            return;

        if (currentWaypoint >= currentPath.vectorPath.Count)
        {
            if (movement == MovementTypes.FollowPath)
            {
                pathCounter++;
                if (pathCounter >= pathNodes.Count)
                    pathCounter = 0;

                FollowNodes();
            }

            return;
        }
            

        Vector2 dir = ((Vector2)currentPath.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = dir * speed * Time.deltaTime;

        transform.parent.position = transform.parent.position + (Vector3)force;

        float distance = Vector2.Distance(rb.position, currentPath.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        FlipSpriteDirection(force);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
        }
    }

    private void FlipSpriteDirection(Vector2 direction)
    {
        if (direction.x >= 0.01f)
            sprite.flipX = false;
        else if (direction.x <= -0.01f)
            sprite.flipX = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && movement == MovementTypes.FollowPlayer)
            target = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && movement == MovementTypes.FollowPlayer)
            target = null;
    }

    private enum MovementTypes
    {
        FollowPlayer,
        FollowPath,
    }
}
