using UnityEngine;
using UnityEngine.InputSystem;

namespace ALICE_PROJECT.PLAYER.INPUT {
    public class PlayerInputHandler {
        private PlayerControls _input;

        [Header("Actions")]
        private InputAction _move;
        private InputAction _jump;
        private InputAction _smashDown;

        public PlayerInputHandler() {
            _input = new PlayerControls();

            _move = _input.Player.Move;
            _move.performed += HandlePlayerMovement;
            _move.canceled += HandlePlayerMovement;

            _jump = _input.Player.Jump;
            _jump.performed += HandlePlayerJump;
            _jump.canceled += HandlePlayerJumpReleased;

            _smashDown = _input.Player.SmashDown;
            _smashDown.performed += HandlePlayerSmashDown;

            _input.Enable();
        }
        public void Dispose() {
            _move.performed -= HandlePlayerMovement;
            _move.canceled -= HandlePlayerMovement;

            _jump.performed -= HandlePlayerJump;
            _jump.canceled -= HandlePlayerJumpReleased;

            _smashDown.performed -= HandlePlayerSmashDown;

            _input.Disable();
        }

        private void HandlePlayerMovement(InputAction.CallbackContext context) {
            Vector2 direction = context.ReadValue<Vector2>();
            InputEvents.RaisePlayerMove(direction);
        }
        private void HandlePlayerJump(InputAction.CallbackContext context) {
            InputEvents.RaisePlayerJump();
        }
        private void HandlePlayerSmashDown(InputAction.CallbackContext context) {
            InputEvents.RaisePlayerSmashDown();
        }
        private void HandlePlayerJumpReleased(InputAction.CallbackContext context) {
            InputEvents.RaisePlayerJumpReleased();
        }
    }
}