using UnityEngine;

namespace MakeMameMay.Core.Player
{
    public class InputManager : MonoBehaviour
    {
        // Input Actions
        private GlobalInputActions inputActions;
        private GlobalInputActions.PlayerActions playerControls;

        // Components
        private Movement.Dash dash;

        private void Awake()
        {
            inputActions = new GlobalInputActions();
            playerControls = inputActions.Player;
        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        private void OnEnable()
        {
            playerControls.Move.Enable();
            playerControls.Dash.Enable();
            playerControls.Dash.performed += _ => dash.PerformDash();
        }

        private void OnDisable()
        {
            playerControls.Move.Disable();
            // playerControls.Dash.Disable();
            // playerControls.Dash.performed -= _ => dash.PerformDash();
        }
    }
}
