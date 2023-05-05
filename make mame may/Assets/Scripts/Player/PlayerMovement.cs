namespace MakeMameMay.Player
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerMovement : MonoBehaviour
    {
        public PlayerController controller;
        public Animator animator;
        public Rigidbody2D rb;
        private PlayerInputActions controls;    // Controls using Unity's InputSystem (newer control scheme)
        private InputAction moveControls;

        private Abilities.DashAbility dash;     // TODO: (not priority) introduce an abilities controller to consolidate all code related to abilities

        // Movement variables
        public float runSpeed = 50f;
        float horizontalMove = 0f;


        private void Awake()
        {
            // Initialise the controls
            controls = new PlayerInputActions();
            moveControls = controls.Player.Move;

            dash = gameObject.AddComponent<Abilities.DashAbility>();

            // Adds a callback function that fires when the controls corresponding to "Dash" is pressed.
            controls.Player.Dash.performed += _ => dash.Trigger();
        }

        private void OnEnable()
        {
            controls.Player.Move.Enable();
            controls.Player.Dash.Enable();
        }

        private void OnDisable()
        {
            controls.Player.Move.Disable();
            controls.Player.Dash.Disable();
        }

        void Update()
        {
            horizontalMove = moveControls.ReadValue<Vector2>().x * runSpeed;

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump"))
            {
                controller.Jump(rb);
                animator.SetBool("IsJumping", true);
            }

            // WallSlide();
            // WallJump();
        }

        public void OnLanding()
        {
            animator.SetBool("IsJumping", false);
        }

        void FixedUpdate()
        {
            // Move character
            // if (!isWallJumping)
            // {
            //     controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
            //     jump = false;
            // }
            controller.Move(rb, horizontalMove);
            controller.Friction(rb);

        }

        #region WallJump
        // // Variables for wall jump
        // [SerializeField] private Transform groundCheck;
        // [SerializeField] private LayerMask groundLayer;
        // [SerializeField] private Transform wallCheck;
        // [SerializeField] private LayerMask wallLayer;
        // private bool isWallDetected;
        // private bool isWallSliding;
        // private float wallSlidingSpeed = 2f;
        // private bool isWallJumping;
        // private float wallJumpingDirection;
        // private float wallJumpingTime = 0.2f;
        // private float wallJumpingCounter;
        // private float wallJumpingDuration = 0.4f;
        // private Vector2 wallJumpingPower = new Vector2(3f, 5f);

        // private bool IsWalled()
        // {
        //     return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        // }

        // private bool IsGrounded()
        // {
        //     return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        // }

        // private void WallSlide()
        // {
        //     if (IsWalled() && !IsGrounded() && horizontalMove != 0f)
        //     {
        //         isWallSliding = true;
        //         rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        //     }
        //     else
        //     {
        //         isWallSliding = false;
        //     }
        // }

        // private void WallJump()
        // {
        //     if (isWallSliding)
        //     {
        //         isWallJumping = false;
        //         wallJumpingDirection = -transform.localScale.x;
        //         wallJumpingCounter = wallJumpingTime;

        //         CancelInvoke(nameof(StopWallJumping));
        //     }
        //     else
        //     {
        //         wallJumpingCounter -= Time.deltaTime;
        //     }

        //     if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        //     {
        //         isWallJumping = true;
        //         Vector2 force = new Vector2(wallJumpingPower.x, wallJumpingPower.y);
        //         force.x *= wallJumpingDirection; //apply force in opposite direction of wall

        //         if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(force.x))
        //         {
        //             force.x -= rb.velocity.x;
        //         }

        //         if (rb.velocity.y < 0)
        //         {
        //             //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
        //             force.y -= rb.velocity.y;
        //         }

        //         // Unlike in the run we want to use the Impulse mode.
        //         // The default mode will apply are force instantly ignoring masss
        //         rb.AddForce(force, ForceMode2D.Impulse);
        //         wallJumpingCounter = 0f;
        //         // if (transform.localScale.x != wallJumpingDirection){
        //         //     // Flip();
        //         // }

        //         Invoke(nameof(StopWallJumping), wallJumpingDuration);
        //     }
        // }

        // private void StopWallJumping()
        // {
        //     isWallJumping = false;
        // }
        #endregion


    }
}