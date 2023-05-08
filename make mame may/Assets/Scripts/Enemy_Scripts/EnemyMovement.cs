namespace Enemy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Pathfinding;

    public class EnemyMovement : MonoBehaviour
    {
        public AIPath aiPath;
        
        private float currDirection;

        void Start()
        {
            currDirection = transform.localScale.x;
        }

        void Update()
        {
            if (aiPath.desiredVelocity.x >= 0.01f) // If player on right, flip right
            {
                transform.localScale = new Vector3(-currDirection, transform.localScale.y, transform.localScale.z);
            } 
            else if (aiPath.desiredVelocity.x <= -0.01f) // Else, flip sprite to left
            {
                transform.localScale = new Vector3(currDirection, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
