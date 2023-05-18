using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace MakeMameMay.Core.Input
{
    /// <summary>
    /// The InputManager reads input from GlobalInputActions and invokes the relevant UnityActions.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_InputManager", menuName = "Game/Input Manager")]
    public class SO_InputManager : ScriptableObject, GlobalInputActions.IPlayerActions
    {
        // Custom player events
        public event UnityAction AttackEvent = delegate { };
        public event UnityAction ShootEvent = delegate { };
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction DashEvent = delegate { };
        public event UnityAction JumpEvent = delegate { };

        // Unity's Input System. Where we retrieve input from.
        private GlobalInputActions _globalInputActions;

        private void OnEnable()
        {
            if (_globalInputActions == null)
            {
                _globalInputActions = new GlobalInputActions();

                _globalInputActions.Player.Enable();
                _globalInputActions.Player.SetCallbacks(this);

                Debug.Log("InputManager: OnEnable()");
            }

        }

        private void OnDisable()
        {
            _globalInputActions.Player.Disable();
            Debug.Log("InputManager: OnDisable()");
        }

        #region Player events
        public void OnAttack(InputAction.CallbackContext context)
        {
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Debug.Log("InputManager: OnMove invoked.");
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnDash(InputAction.CallbackContext context)
        {
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            Debug.Log("InputManager: OnJump invoked.");
            JumpEvent.Invoke();
        }
        #endregion
    }
}
