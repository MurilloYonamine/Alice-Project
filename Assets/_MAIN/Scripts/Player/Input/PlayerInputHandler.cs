using UnityEngine;
using UnityEngine.InputSystem;

namespace PLAYER.INPUT {
    public class PlayerInputHandler {
        private PlayerControls _input;
        private InputAction _move;
        private InputAction _jump;
        public PlayerInputHandler() {
            _input = new PlayerControls();

            _move = _input.Player.Move;
            _move.performed += HandlePlayerMovement;
            _move.canceled += HandlePlayerMovement;

            _jump = _input.Player.Jump;
            _jump.performed += HandlePlayerJump;
            _jump.canceled += HandlePlayerJumpReleased;

            _input.Enable();
        }
        public void Dispose() {
            _move.performed -= HandlePlayerMovement;
            _move.canceled -= HandlePlayerMovement;

            _jump.performed -= HandlePlayerJump;
            _jump.canceled -= HandlePlayerJumpReleased;

            _input.Disable();
        }

        private void HandlePlayerMovement(InputAction.CallbackContext context) {
            Vector2 direction = context.ReadValue<Vector2>();
            InputEvents.RaisePlayerMove(direction);
        }
        private void HandlePlayerJump(InputAction.CallbackContext context) {
            InputEvents.RaisePlayerJump();
        }
        private void HandlePlayerJumpReleased(InputAction.CallbackContext context) {
            InputEvents.RaisePlayerJumpReleased();
        }
    }
}