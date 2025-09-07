using UnityEngine;
using UnityEngine.InputSystem;
using PLAYER.INPUT;

namespace PLAYER.MOBILE
{
    public class TouchJumpDetector : MonoBehaviour
    {
        [Header("Double Tap Settings")]
        [SerializeField] private float doubleTapTime = 0.3f;
        [SerializeField] private float maxTapDistance = 100f;

        private float lastTapTime = 0f;
        private Vector2 lastTapPosition;
        private bool isWaitingForSecondTap = false;

        private void Update()
        {
            // Só funciona em dispositivos móveis
            if (!Application.isMobilePlatform) return;

            // Detecta toques na tela
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                float currentTime = Time.time;

                if (isWaitingForSecondTap)
                {
                    // Verifica se é um double tap válido
                    float timeDifference = currentTime - lastTapTime;
                    float distanceDifference = Vector2.Distance(touchPosition, lastTapPosition);

                    if (timeDifference <= doubleTapTime && distanceDifference <= maxTapDistance)
                    {
                        // Double tap detectado!
                        OnDoubleTapDetected();
                        isWaitingForSecondTap = false;
                    }
                    else
                    {
                        // Muito tempo passou ou muito longe, reinicia
                        StartNewTapSequence(touchPosition, currentTime);
                    }
                }
                else
                {
                    // Primeiro tap
                    StartNewTapSequence(touchPosition, currentTime);
                }
            }

            // Timeout para o segundo tap
            if (isWaitingForSecondTap && Time.time - lastTapTime > doubleTapTime)
            {
                isWaitingForSecondTap = false;
            }
        }

        private void StartNewTapSequence(Vector2 position, float time)
        {
            lastTapPosition = position;
            lastTapTime = time;
            isWaitingForSecondTap = true;
        }

        private void OnDoubleTapDetected()
        {
            InputEvents.RaisePlayerTouchJump();
        }
    }
}
