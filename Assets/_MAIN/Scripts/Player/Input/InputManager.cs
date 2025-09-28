using UnityEngine;

namespace ALICE_PROJECT.PLAYER.INPUT {
    public class InputManager : MonoBehaviour {
        public static PlayerControls Controls;

        private void Awake() {
            if (Controls == null) {
                Controls = new PlayerControls();
                Controls.Enable();
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}