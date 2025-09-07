using UnityEngine;
using UnityEngine.InputSystem;

namespace PLAYER.INPUT {
    public class PlayerInputHandler {
        private PlayerControls _input;
        private InputAction _move;
        private InputAction _jump;
        private InputAction _touchMove;
        private InputAction _touchJump;
        
        public PlayerInputHandler() {
            _input = new PlayerControls();

            _move = _input.Player.Move;
            _move.performed += HandlePlayerMovement;
            _move.canceled += HandlePlayerMovement;

            _jump = _input.Player.Jump;
            _jump.performed += HandlePlayerJump;
            _jump.canceled += HandlePlayerJumpReleased;

            // Touch controls
            _touchMove = _input.Player.TouchMove;
            _touchMove.performed += HandlePlayerTouchMovement;
            _touchMove.canceled += HandlePlayerTouchMovement;

            _touchJump = _input.Player.TouchJump;
            _touchJump.performed += HandlePlayerTouchJump;

            _input.Enable();
        }
        
        public void Dispose() {
            _move.performed -= HandlePlayerMovement;
            _move.canceled -= HandlePlayerMovement;

            _jump.performed -= HandlePlayerJump;
            _jump.canceled -= HandlePlayerJumpReleased;

            _touchMove.performed -= HandlePlayerTouchMovement;
            _touchMove.canceled -= HandlePlayerTouchMovement;

            _touchJump.performed -= HandlePlayerTouchJump;

            _input.Disable();
        }

        private void HandlePlayerMovement(InputAction.CallbackContext context) {
            Vector2 direction = context.ReadValue<Vector2>();
            InputEvents.RaisePlayerMove(direction);
        }
        
        private void HandlePlayerTouchMovement(InputAction.CallbackContext context) {
            Vector2 direction = context.ReadValue<Vector2>();
            InputEvents.RaisePlayerTouchMove(direction);
        }
        
        private void HandlePlayerJump(InputAction.CallbackContext context) {
            InputEvents.RaisePlayerJump();
        }
        
        private void HandlePlayerTouchJump(InputAction.CallbackContext context) {
            InputEvents.RaisePlayerTouchJump();
        }
        
        private void HandlePlayerJumpReleased(InputAction.CallbackContext context) {
            InputEvents.RaisePlayerJumpReleased();
        }
    }
}