using UnityEngine;
using PLAYER.INPUT;

namespace PLAYER.MOBILE
{
    public class InputPlatformManager : MonoBehaviour
    {
        [Header("Input Platform Settings")]
        [SerializeField] private GameObject mobileInputUI;
        [SerializeField] private MobileInputController mobileController;
        [SerializeField] private TouchJumpDetector touchJumpDetector;
        [SerializeField] private MonoBehaviour fullScreenJoystick; 
        [SerializeField] private bool useFullScreenJoystick = true;

        private void Awake()
        {
            SetupInputBasedOnPlatform();
        }

        private void SetupInputBasedOnPlatform()
        {
            if (Application.isMobilePlatform || Application.isEditor)
            {
                SetupMobileInput();
                Debug.Log("Mobile platform detected - Mobile controls enabled");
            }
            else
            {
                SetupDesktopInput();
                Debug.Log("Desktop platform detected - Keyboard/Mouse controls enabled");
            }
        }

        private void SetupMobileInput()
        {
            if (useFullScreenJoystick)
            {
                SetupFullScreenJoystick();
            }
            else
            {
                SetupTraditionalMobileInput();
            }
        }

        private void SetupFullScreenJoystick()
        {
            // Desativa UI mobile tradicional
            if (mobileInputUI != null)
                mobileInputUI.SetActive(false);

            if (mobileController != null)
                mobileController.enabled = false;

            if (touchJumpDetector != null)
                touchJumpDetector.enabled = false;

            // Ativa full screen joystick
            if (fullScreenJoystick != null)
                fullScreenJoystick.enabled = true;
            
            Debug.Log("Full Screen Joystick enabled - Touch anywhere to move, tap to jump!");
        }

        private void SetupTraditionalMobileInput()
        {
            // Ativa UI mobile tradicional
            if (mobileInputUI != null)
                mobileInputUI.SetActive(true);

            if (mobileController != null)
                mobileController.enabled = true;

            if (touchJumpDetector != null)
                touchJumpDetector.enabled = true;

            // Desativa full screen joystick
            if (fullScreenJoystick != null)
                fullScreenJoystick.enabled = false;
        }

        private void SetupDesktopInput()
        {
            // Desativa toda UI mobile
            if (mobileInputUI != null)
                mobileInputUI.SetActive(false);

            // Desativa todos controladores mobile
            if (mobileController != null)
                mobileController.enabled = false;

            if (touchJumpDetector != null)
                touchJumpDetector.enabled = false;

            if (fullScreenJoystick != null)
                fullScreenJoystick.enabled = false;
        }

        public void ToggleMobileInputMode()
        {
            useFullScreenJoystick = !useFullScreenJoystick;
            SetupMobileInput();
            
            string mode = useFullScreenJoystick ? "Full Screen Joystick" : "Traditional UI";
            Debug.Log($"Switched to {mode} mode");
        }

        public void ForceSetPlatform(bool isMobile)
        {
            if (isMobile)
                SetupMobileInput();
            else
                SetupDesktopInput();
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.M))
            {
                ForceSetPlatform(true);
                Debug.Log("Switched to Mobile Input");
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                ForceSetPlatform(false);
                Debug.Log("Switched to Desktop Input");
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                ToggleMobileInputMode();
            }
            #endif
        }
    }
}
