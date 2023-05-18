using UnityEngine;

namespace MakeMameMay.Core.Player
{
    /// <summary>
    /// The PlayerController handles basic movement.
    /// </summary>
    public class MovementManager : MonoBehaviour
    {
        #region Settings

        [Header("Run Settings")]
        [SerializeField] private float acceleration = 1f;
        private float decelerationFactor = -2f;
        [SerializeField] private float maxSpeed = .25f;
        [SerializeField] private float velocityPower = 2f;
        [SerializeField] private float runSpeed = 50f;
        private bool isFacingRight = true;
        private float horizontalDirection;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 15f;
        [SerializeField] Transform groundCheckPoint;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] Vector2 groundCheckSize = new Vector2(.75f, .1f);

        #endregion

        public void Move(Rigidbody2D rb, float direction)
        {
            direction *= runSpeed;
            float targetSpeed = direction * maxSpeed;
            float speedDiff = targetSpeed - rb.velocity.x;
            // Accelerate or decelerate depending on desired movement direction
            float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : acceleration * decelerationFactor;
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelerationRate, velocityPower) * Mathf.Sign(speedDiff);

            rb.AddForce(movement * Vector2.right);

            if (direction > 0f && !isFacingRight) { Flip(); }
            else if (direction < 0f && isFacingRight) { Flip(); }
        }

        public void Friction(Rigidbody2D rb)
        {
            float friction = Mathf.Min(Mathf.Abs(rb.velocity.x), 0.2f);
            friction *= -Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * friction, ForceMode2D.Impulse);
        }

        public void Jump(Rigidbody2D rb)
        {
            if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        private void Flip()
        {
            transform.Rotate(0f, 180f, 0f);
            isFacingRight = !isFacingRight;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
        }
    }
}