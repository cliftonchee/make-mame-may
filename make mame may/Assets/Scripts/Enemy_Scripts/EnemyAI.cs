namespace Enemy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Pathfinding;

    public class EnemyAI : MonoBehaviour
    {
        [Header("Pathfinding")]
        public Transform target;
        public float activateDistance = 50f; // Distance for enemy to start moving
        public float pathUpdateSeconds = 0.5f; // How often it updates

        [Header("Physics")]
        public float speed = 200f;
        public float nextWaypointDistance = 3f;
        public float jumpNodeHeightRequirement = 0.8f; // Difference in height for enemy to jump
        public float jumpModifier = 0.3f; // How much it jumps
        public float jumpCheckOffset = 0.1f; // Checks if colliding

        [Header("Custom Behaviour")]
        public bool followEnabled = true;
        public bool jumpEnabled = true;
        public bool directionLookEnabled = true;

        private Path path;
        private int currentWaypoint = 0;
        bool isGrounded = false;
        Seeker seeker;
        Rigidbody2D rb;

        public void Start()
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();

            InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
        }

        private void FixedUpdate()
        {
            if (TargetInDistance() && followEnabled)
            {
                PathFollow();
            }
        }

        private void UpdatePath()
        {
            if (followEnabled && TargetInDistance() && seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
        }

        private void PathFollow()
        {
            if (path == null)
            {
                return;
            }

            // End of path
            if (currentWaypoint >= path.vectorPath.Count)
            {
                return;
            }

            // See if colliding
            Vector3 startOffset = transform.position - new Vector3(0f,
                                    GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
            isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

            // Calculate direction
            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position)
                                    .normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            // Jump function
            if (jumpEnabled && isGrounded)
            {
                if (direction.y > jumpNodeHeightRequirement)
                {
                    rb.AddForce(Vector2.up * speed * jumpModifier);
                }
            }

            // Movement
            rb.AddForce(force);

            // Next waypoint
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            // Directions Graphics Handling
            if (directionLookEnabled)
            {
                if (rb.velocity.x > 0.05f)
                {
                    transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x)
                                                ,transform.localScale.y, transform.localScale.z);
                }
                else if (rb.velocity.x < -0.05f)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)
                                                ,transform.localScale.y, transform.localScale.z);
                }
            }
        }

        private bool TargetInDistance()
        {
            return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
        }

        private void OnPathComplete(Path p) 
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }
    }
}