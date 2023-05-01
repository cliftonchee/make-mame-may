using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public Rigidbody2D rb2D;

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    // TODO: Lock character's y position when dashing
    // TODO: Double jump implementation

    // Variables for dash implementation
    float dashForce = 500f;
    float maxDashDistance = .1f;
    float dashCooldown = 2f; // Cooldown time in seconds
    float remainingDashCooldown = 0f;
    bool isDashing = false;
    float distanceTraveled = 0f;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        if (remainingDashCooldown <= 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            {
                isDashing = true;
            }
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouch(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    void FixedUpdate()
    {
        // Move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

        if (isDashing)
        {
            Vector2 dashDirection = transform.right * Mathf.Sign(transform.localScale.x);
            rb2D.AddForce(dashDirection * dashForce);

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
