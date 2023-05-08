namespace Player.Abilities
{

    using UnityEngine;
    using System.Collections;

    public class WallJump : MonoBehaviour
    {
        public Rigidbody2D rb2d;
        public CharacterController2D controller;

        public bool isWalled = false;
        public bool isWallSliding;
        public bool isWallJumping;

        public bool wallOnRight = false;
        public bool wallOnLeft = false;

        // Variables for wall jump
        public Transform GroundCheck;
        public LayerMask GroundLayer;
        public Transform WallCheck;
        public LayerMask WallLayer;

        private float wallSlidingSpeed = 5f;
        private Vector2 wallJumpingPower = new Vector2(5f, 10f);

        private float wallJumpCooldown = 0.25f;
        private float lastWallJumpTime;

        private float wallJumpingDuration = 0.2f;

        private void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            controller = GetComponent<CharacterController2D>();
        }
        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                wallJump();
            }
            lastWallJumpTime += Time.deltaTime;
            WallSlide();
        }

        private int WallDirection()
        {
            isWalled = IsWalledForWallJump() || IsWalledForWallSlide();
            if (isWalled && Mathf.RoundToInt(transform.rotation.eulerAngles.y) == 180)
            {
                wallOnRight = false;
                wallOnLeft = true;
                return -1;
            }
            else if (isWalled && transform.rotation.eulerAngles.y < 0.1)
            {
                wallOnRight = true;
                wallOnLeft = false;
                return 1;
            }
            else
            {
                wallOnLeft = false;
                wallOnRight = false;
                return 0;
            }
        }

        private bool IsWalledForWallSlide()
        {
            return Physics2D.OverlapCircle(WallCheck.position, 0.2f, WallLayer);
        }

        private bool IsWalledForWallJump()
        {
            return Physics2D.OverlapCircle(WallCheck.position, 0.3f, WallLayer);
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
        }

        private void WallSlide()
        {
            if (IsWalledForWallSlide() && !IsGrounded() && !isWallJumping)
            {
                if (Input.GetAxisRaw("Horizontal") != 0f && !isWallSliding)
                {
                    isWallSliding = true;
                    //make character look opp direction of wall
                    rb2d.velocity = new Vector2(0, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed + 2, float.MaxValue));
                }
                else if (isWallSliding)
                {
                    if (Input.GetAxisRaw("Horizontal") != 0)
                    {
                        if (Input.GetAxisRaw("Horizontal") != WallDirection())
                        {
                            rb2d.velocity = new Vector2(0, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
                            //timer for coyote time
                            StartCoroutine(WallSlideCoroutine());
                        }
                        else
                        {
                            rb2d.velocity = new Vector2(0, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed + 2, float.MaxValue));
                        }
                    }
                    else
                    {
                        rb2d.velocity = new Vector2(0, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
                    }


                }
            }
            else
            {
                isWallSliding = false;
            }
        }

        private void wallJump()
        {
            if (IsWalledForWallJump() && !IsGrounded())
            {
                isWallJumping = false;
                CancelInvoke(nameof(StopWallJumping));

                if (Input.GetButtonDown("Jump") && lastWallJumpTime > wallJumpCooldown)
                {
                    int jumpDirection = -WallDirection();
                    lastWallJumpTime = 0;
                    isWallSliding = false;
                    isWallJumping = true;
                    controller.Flip();

                    if (Input.GetAxisRaw("Horizontal") != jumpDirection)
                    {
                        rb2d.velocity = new Vector2(jumpDirection * wallJumpingPower.x, Mathf.Abs(wallJumpingPower.y));
                        StartCoroutine(WallJumpCoroutine(jumpDirection));

                    }
                    else if (Input.GetAxisRaw("Horizontal") == jumpDirection)
                    {
                        Vector2 force = new Vector2(jumpDirection * wallJumpingPower.x * 3, Mathf.Abs(wallJumpingPower.y) * 1.5f);
                        rb2d.AddForce(force, ForceMode2D.Impulse);
                    }

                }
                Invoke(nameof(StopWallJumping), wallJumpingDuration);
            }
        }

        private void StopWallJumping()
        {
            lastWallJumpTime += Time.deltaTime;
            if (!IsGrounded() && !IsWalledForWallJump())
            {
                isWallJumping = false;
            }

        }
        IEnumerator WallJumpCoroutine(int jumpDirection)
        {
            yield return new WaitForSeconds(0.2f);
            rb2d.velocity = new Vector2(-jumpDirection * wallJumpingPower.x, Mathf.Abs(wallJumpingPower.y));
        }

        IEnumerator WallSlideCoroutine()
        {
            yield return new WaitForSeconds(0.2f);
            isWallSliding = false;
        }

    }

}