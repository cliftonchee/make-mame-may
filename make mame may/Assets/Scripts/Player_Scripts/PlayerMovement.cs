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

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private bool isWallDetected;
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(3f, 5f);

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
        WallSlide();
        WallJump();
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
        if(!isWallJumping){
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
            jump = false;
        }

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

    private bool IsWalled(){
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private bool IsGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void WallSlide(){
        if (IsWalled() && !IsGrounded() && horizontalMove != 0f)
        {
            isWallSliding = true;
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else{
            isWallSliding = false;
        }
    }

    private void WallJump(){
        if (isWallSliding){
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else{
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f){
            isWallJumping = true;
            rb2D.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection){
                Flip();
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping(){
        isWallJumping = false;
        Flip();
    }

    private void Flip(){
        Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }
}
