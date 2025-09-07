using UnityEngine;
using PLAYER.INPUT;

namespace PLAYER.MOBILE
{
    /// <summary>
    /// Preset configurations for different types of mobile controls
    /// Attach this to the same GameObject as FullScreenJoystick for easy setup
    /// </summary>
    public class FullScreenJoystickPresets : MonoBehaviour
    {
        [Header("Preset Selection")]
        [SerializeField] private JoystickPreset selectedPreset = JoystickPreset.Balanced;
        
        private FullScreenJoystick joystick;

        public enum JoystickPreset
        {
            Casual,      // Mais fácil, menos sensível
            Balanced,    // Equilibrado para a maioria
            Responsive,  // Mais sensível e responsivo
            Precision,   // Para controle preciso
            Custom       // Configuração manual
        }

        private void Start()
        {
            joystick = GetComponent<FullScreenJoystick>();
            if (joystick != null)
            {
                ApplyPreset(selectedPreset);
            }
        }

        public void ApplyPreset(JoystickPreset preset)
        {
            if (joystick == null) return;

            switch (preset)
            {
                case JoystickPreset.Casual:
                    ApplyValues(20f, 0.05f, 150f, 0.4f);
                    Debug.Log("Applied Casual preset - Easy and forgiving controls");
                    break;

                case JoystickPreset.Balanced:
                    ApplyValues(10f, 0.1f, 200f, 0.3f);
                    Debug.Log("Applied Balanced preset - Good for most players");
                    break;

                case JoystickPreset.Responsive:
                    ApplyValues(5f, 0.15f, 250f, 0.2f);
                    Debug.Log("Applied Responsive preset - Fast and sensitive");
                    break;

                case JoystickPreset.Precision:
                    ApplyValues(8f, 0.08f, 300f, 0.25f);
                    Debug.Log("Applied Precision preset - Accurate control");
                    break;

                case JoystickPreset.Custom:
                    Debug.Log("Custom preset selected - Configure manually");
                    break;
            }

            selectedPreset = preset;
        }

        private void ApplyValues(float threshold, float sensitivity, float maxDistance, float tapTime)
        {
            // Using reflection to set private fields
            var type = typeof(FullScreenJoystick);
            
            var thresholdField = type.GetField("moveThreshold", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var sensitivityField = type.GetField("moveSensitivity", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var maxDistanceField = type.GetField("maxMoveDistance", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var tapTimeField = type.GetField("tapTimeThreshold", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            thresholdField?.SetValue(joystick, threshold);
            sensitivityField?.SetValue(joystick, sensitivity);
            maxDistanceField?.SetValue(joystick, maxDistance);
            tapTimeField?.SetValue(joystick, tapTime);
        }

        // Métodos públicos para usar em UI ou eventos
        public void SetCasualMode() => ApplyPreset(JoystickPreset.Casual);
        public void SetBalancedMode() => ApplyPreset(JoystickPreset.Balanced);
        public void SetResponsiveMode() => ApplyPreset(JoystickPreset.Responsive);
        public void SetPrecisionMode() => ApplyPreset(JoystickPreset.Precision);

        // Para debug no editor
        private void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha1)) SetCasualMode();
            if (Input.GetKeyDown(KeyCode.Alpha2)) SetBalancedMode();
            if (Input.GetKeyDown(KeyCode.Alpha3)) SetResponsiveMode();
            if (Input.GetKeyDown(KeyCode.Alpha4)) SetPrecisionMode();
            #endif
        }

        private void OnValidate()
        {
            // Aplica preset quando mudado no Inspector
            if (Application.isPlaying && joystick != null)
            {
                ApplyPreset(selectedPreset);
            }
        }
    }
}
