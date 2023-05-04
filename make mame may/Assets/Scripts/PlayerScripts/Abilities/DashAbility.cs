namespace Player
{
    namespace Abilities
    {
        using UnityEngine;

        public class DashAbility : MonoBehaviour
        {
            // TODO: Lock character's y position when dashing
            // TODO: Double jump implementation

            float dashForce = 500f;
            float maxDashDistance = .1f;
            float dashCooldown = 2f; // Cooldown time in seconds
            float remainingDashCooldown = 0f;
            bool isDashing = false;
            float distanceTraveled = 0f;

            public void Trigger()
            {
                if (remainingDashCooldown <= 0f && !isDashing)
                {
                    isDashing = true;
                }
            }

            void FixedUpdate()
            {
                if (isDashing)
                {
                    Vector2 dashDirection = transform.right * Mathf.Sign(transform.localScale.x);
                    GetComponent<Rigidbody2D>().AddForce(dashDirection * dashForce);

                    distanceTraveled += dashDirection.magnitude * Time.fixedDeltaTime;

                    // After travelling the specified dash distance
                    if (distanceTraveled >= maxDashDistance)
                    {
                        isDashing = false;
                        distanceTraveled = 0f;
                        remainingDashCooldown = dashCooldown; // Start the cooldown timer
                    }
                }

                if (remainingDashCooldown > 0f)
                {
                    remainingDashCooldown -= Time.fixedDeltaTime;
                }
            }
        }

    }
}
