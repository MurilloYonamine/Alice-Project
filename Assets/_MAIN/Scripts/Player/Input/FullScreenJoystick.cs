using UnityEngine;
using UnityEngine.InputSystem;
using PLAYER.INPUT;

namespace PLAYER.MOBILE
{
    public class FullScreenJoystick : MonoBehaviour
    {
        [Header("Full Screen Joystick Settings")]
        [SerializeField] private float moveThreshold = 10f; // Distância mínima para começar a mover
        [SerializeField] private float moveSensitivity = 0.1f; // Sensibilidade do movimento
        [SerializeField] private float maxMoveDistance = 200f; // Distância máxima para movimento máximo

        [Header("Jump Settings")]
        [SerializeField] private float tapTimeThreshold = 0.3f; // Tempo máximo para considerar um tap
        
        private Vector2 touchStartPosition;
        private Vector2 currentTouchPosition;
        private bool isTouching = false;
        private bool isMoving = false;
        private float touchStartTime;
        
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // Só funciona em dispositivos móveis
            if (!Application.isMobilePlatform && !Application.isEditor) return;

            HandleTouchInput();
        }

        private void HandleTouchInput()
        {
            // Usando o novo Input System
            if (Touchscreen.current != null)
            {
                var primaryTouch = Touchscreen.current.primaryTouch;
                
                // Toque começou
                if (primaryTouch.press.wasPressedThisFrame)
                {
                    OnTouchStart(primaryTouch.position.ReadValue());
                }
                // Toque está ativo (arraste)
                else if (primaryTouch.press.isPressed)
                {
                    OnTouchDrag(primaryTouch.position.ReadValue());
                }
                // Toque terminou
                else if (primaryTouch.press.wasReleasedThisFrame)
                {
                    OnTouchEnd();
                }
            }

            // Fallback para mouse no editor
            #if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                OnTouchStart(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                OnTouchDrag(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnTouchEnd();
            }
            #endif
        }

        private void OnTouchStart(Vector2 touchPosition)
        {
            touchStartPosition = touchPosition;
            currentTouchPosition = touchPosition;
            isTouching = true;
            isMoving = false;
            touchStartTime = Time.time;
        }

        private void OnTouchDrag(Vector2 touchPosition)
        {
            if (!isTouching) return;

            currentTouchPosition = touchPosition;
            Vector2 deltaPosition = currentTouchPosition - touchStartPosition;
            float distance = deltaPosition.magnitude;

            // Se moveu além do threshold, começa a mover
            if (distance > moveThreshold && !isMoving)
            {
                isMoving = true;
            }

            if (isMoving)
            {
                // Calcula direção e intensidade do movimento
                Vector2 moveDirection = deltaPosition.normalized;
                float moveIntensity = Mathf.Clamp01(distance / maxMoveDistance);

                // Só movimento horizontal para o player
                Vector2 moveInput = new Vector2(moveDirection.x * moveIntensity, 0);

                // Aplica sensibilidade
                moveInput *= moveSensitivity;

                // Envia o input para o sistema
                InputEvents.RaisePlayerTouchMove(moveInput);
            }
        }

        private void OnTouchEnd()
        {
            if (!isTouching) return;

            float touchDuration = Time.time - touchStartTime;
            Vector2 deltaPosition = currentTouchPosition - touchStartPosition;
            float distance = deltaPosition.magnitude;

            // Se foi um toque rápido e curto, considera como pulo
            if (touchDuration <= tapTimeThreshold && distance <= moveThreshold)
            {
                InputEvents.RaisePlayerJump();
            }

            // Para o movimento
            if (isMoving)
            {
                InputEvents.RaisePlayerTouchMove(Vector2.zero);
            }

            // Reset do estado
            isTouching = false;
            isMoving = false;
        }
    }
}
