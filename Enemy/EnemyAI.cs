using System;
using System.Dynamic;
using Pathfinding;
using UnityEngine;
public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200;
    public float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool isEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 velocity;
    private Vector2 previousVelocity;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            isEndOfPath = true;
            return;
        }
        else
        {
            isEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;

        previousVelocity = velocity;
        velocity = rb.velocity;
        SetAnimations();

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        rb.velocity = force;

        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }

    private void SetAnimations()
    {
        animator.SetFloat(AnimationParameters.SPEED, velocity.magnitude);

        if (velocity.normalized.x > 0.5)
        {
            animator.SetTrigger(AnimationParameters.RIGHT);
        }
        else if (velocity.normalized.x < -0.5)
        {
            animator.SetTrigger(AnimationParameters.LEFT);
        }
        else if (velocity.normalized.y > 0.5)
        {
            animator.SetTrigger(AnimationParameters.UP);
        }
        else if (velocity.normalized.y < -0.5)
        {
            animator.SetTrigger(AnimationParameters.DOWN);
        }
    }
}