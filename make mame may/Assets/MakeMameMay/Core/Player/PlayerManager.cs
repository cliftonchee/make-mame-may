using UnityEngine;
using UnityEngine.InputSystem;
using MakeMameMay.Core.Input;

namespace MakeMameMay.Core.Player
{
    public class PlayerManager : MonoBehaviour
    {
        // Manager scripts
        public MovementManager movementManager;
        [SerializeField] SO_InputManager _inputManager = default;

        // Components
        public Animator animator;
        public Rigidbody2D rb;

        private void OnEnable()
        {
            _inputManager.MoveEvent += OnMove;
        }

        private void Awake()
        {
        }

        void Update()
        {
            movementManager.Friction(rb);
            // animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            // if (UnityEngine.Input.GetButtonDown("Jump"))
            // {
            //     movementManager.Jump(rb);
            //     animator.SetBool("IsJumping", true);
            // }
        }

        public void OnLanding()
        {
            // animator.SetBool("IsJumping", false);
        }

        private void OnMove(Vector2 movement)
        {
            movementManager.Move(rb, movement.x);
        }

    }
}