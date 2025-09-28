using ALICE_PROJECT.PLAYER.INPUT;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ALICE_PROJECT.SCREEN {
    public class TouchScreenMovement : MonoBehaviour {
        [SerializeField] private GameObject LeftScreen;
        [SerializeField] private GameObject RightScreen;
        [SerializeField] private Camera _camera;

        private PlayerControls _input;
        private InputAction _touch;
        private Vector2 _touchMovement = Vector2.zero;

        private void OnEnable() {
            _input = new PlayerControls();
            _touch = _input.Player.Touch;
            _input.Enable();
        }

        private void OnDisable() {
            _input.Disable();
        }

        private void Update() {
            var touches = Touchscreen.current.touches;

            bool moveLeft = false;
            bool moveRight = false;

            foreach (var touch in touches) {
                if (!touch.press.isPressed) continue;

                Vector2 screenPosition = touch.position.ReadValue();
                Vector2 worldPosition = _camera.ScreenToWorldPoint(screenPosition);

                if (worldPosition.x < LeftScreen.transform.position.x) {
                    moveLeft = true;
                }
                else if (worldPosition.x > RightScreen.transform.position.x) {
                    moveRight = true;
                }
            }

            if (moveLeft && !moveRight) {
                _touchMovement = Vector2.left;
            }
            else if (moveRight && !moveLeft) {
                _touchMovement = Vector2.right;
            }
            else {
                _touchMovement = Vector2.zero;
            }

            InputEvents.RaisePlayerMove(_touchMovement);
        }
    }
}