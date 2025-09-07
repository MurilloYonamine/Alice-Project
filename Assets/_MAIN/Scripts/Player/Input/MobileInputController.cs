using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PLAYER.INPUT;

namespace PLAYER.MOBILE
{
    public class MobileInputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Header("Mobile Input Settings")]
        [SerializeField] private RectTransform joystickArea;
        [SerializeField] private RectTransform joystickHandle;
        [SerializeField] private Button jumpButton;
        [SerializeField] private float joystickRange = 50f;
        [SerializeField] private float moveMultiplier = 2f;

        private Vector2 joystickCenter;
        private Vector2 inputVector;
        private bool isDragging = false;
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
            SetupMobileControls();
        }

        private void SetupMobileControls()
        {
            // Só ativa controles mobile se estiver em dispositivo móvel
            if (Application.isMobilePlatform)
            {
                if (joystickArea != null)
                    joystickArea.gameObject.SetActive(true);
                if (jumpButton != null)
                {
                    jumpButton.gameObject.SetActive(true);
                    jumpButton.onClick.AddListener(OnJumpButtonPressed);
                }
            }
            else
            {
                // Desativa controles mobile em PC
                if (joystickArea != null)
                    joystickArea.gameObject.SetActive(false);
                if (jumpButton != null)
                    jumpButton.gameObject.SetActive(false);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
            joystickCenter = eventData.position;
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
            inputVector = Vector2.zero;
            
            // Retorna o handle para o centro
            if (joystickHandle != null)
                joystickHandle.anchoredPosition = Vector2.zero;
            
            // Para o movimento quando solta o toque
            InputEvents.RaisePlayerTouchMove(Vector2.zero);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging) return;

            Vector2 direction = eventData.position - joystickCenter;
            float distance = Vector2.Distance(eventData.position, joystickCenter);

            // Limita a distância do joystick
            if (distance > joystickRange)
            {
                direction = direction.normalized * joystickRange;
                distance = joystickRange;
            }

            // Atualiza a posição visual do handle
            if (joystickHandle != null)
                joystickHandle.anchoredPosition = direction;

            // Calcula o input normalizado
            inputVector = direction / joystickRange;
            inputVector.x *= moveMultiplier;
            inputVector.y = 0; // Não queremos movimento vertical

            // Envia o input para o sistema
            InputEvents.RaisePlayerTouchMove(inputVector);
        }

        private void OnJumpButtonPressed()
        {
            InputEvents.RaisePlayerTouchJump();
        }

        private void OnDestroy()
        {
            if (jumpButton != null)
                jumpButton.onClick.RemoveListener(OnJumpButtonPressed);
        }
    }
}
