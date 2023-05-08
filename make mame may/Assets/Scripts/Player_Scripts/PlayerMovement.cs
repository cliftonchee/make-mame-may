namespace Player
{
    using UnityEngine;
    using System.Collections;

    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController2D controller;
        public Animator animator;
        public Rigidbody2D rb2d;
        private PlayerInputActions controls;    // Controls using Unity's InputSystem (newer control scheme)
        private Abilities.Dash dash;
        private Abilities.WallJump walljump;           // TODO: (not priority) introduce an abilities controller to consolidate all code related to abilities

        // Movement variables
        public float runSpeed = 40f;
        float horizontalMove = 0f;
        bool jump = false;
        bool crouch = false;

        // Variables for wall jump
        [SerializeField] public Transform groundCheck;
        [SerializeField] public LayerMask groundLayer;
        [SerializeField] public Transform wallCheck;
        [SerializeField] public LayerMask wallLayer;


        private void Awake()
        {
            // Initialise the controls
            controls = new PlayerInputActions();

            dash = gameObject.AddComponent<Abilities.Dash>();
            walljump = gameObject.AddComponent<Abilities.WallJump>();

            walljump.GroundCheck = groundCheck;
            walljump.GroundLayer = groundLayer;
            walljump.WallCheck = wallCheck;
            walljump.WallLayer = wallLayer;

            // Adds a callback function that fires when the controls corresponding to "Dash" is pressed.
            controls.Player.Dash.performed += _ => dash.Trigger();
        }

        private void OnEnable()
        {
            controls.Player.Dash.Enable();
        }

        private void OnDisable()
        {
            controls.Player.Dash.Disable();
        }

        void Update()
        {
            if (!walljump.isWallJumping && !walljump.isWallSliding)
            {
                horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            }

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
            if (!walljump.isWallJumping && !walljump.isWallSliding)
            {
                controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
                jump = false;
            }

        }

    }
}