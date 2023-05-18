using UnityEngine;

namespace MakeMameMay.Core.Player.Movement
{
    public class Dash : MonoBehaviour
    {
        float dashForce = 500f;
        float maxDashDistance = .1f;
        float dashCooldown = 2f;
        float remainingDashCooldown = 0f;
        bool isDashing = false;
        float distanceTraveled = 0f;

        public void PerformDash()
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
