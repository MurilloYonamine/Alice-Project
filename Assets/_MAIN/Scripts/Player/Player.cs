using LEVELGENERATOR;
using PLAYER.INPUT;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PLAYER {
    public class Player : MonoBehaviour {
        private PlayerControls _input;
        private InputAction _move;
        private InputAction _generate;

        private Rigidbody2D _rigidbody2D;

        [Header("Generation Control")]
        private int _lastHeight;
        private int _lastMid;
        private int _levelSize;

        private void Awake() {
            _input = new PlayerControls();
            _rigidbody2D = GetComponent<Rigidbody2D>();

        }
        private void Start() {
            _levelSize = LevelGeneratorManager.Instance.LevelSize;
        }
        private void Update() {
            GetCurrentLocation();
        }
        private void OnEnable() {
            _move = _input.Player.Move;
            _generate = _input.Player.GenerateChunk;

            _move.Enable();
            _move.performed += HandleMovement;

            _generate.Enable();
            _generate.performed += HandleLevelGeneration;
        }
        private void OnDisable() {
            _move.Disable();
            _move.performed -= HandleMovement;

            _generate.Disable();
            _generate.performed -= HandleLevelGeneration;
        }
        private void HandleMovement(InputAction.CallbackContext context) {
            Vector2 direction = context.ReadValue<Vector2>();
        }
        private void HandleLevelGeneration(InputAction.CallbackContext context) {
            LevelEvents.PlayerAdvanceChunk();
        }
        private void GetCurrentLocation() {
            int tilemapHeight = LevelGeneratorManager.Instance.EmptyTilemap.size.y;
            Vector3Int playerWorldPosition = Vector3Int.RoundToInt(transform.position);
            Vector3Int playerCellPosition = Vector3Int.RoundToInt(LevelGeneratorManager.Instance.EmptyTilemap.WorldToCell(playerWorldPosition));

            int playerY = playerCellPosition.y - 9;
            int currentMid = tilemapHeight == _levelSize ? tilemapHeight / 2 : _levelSize + _lastMid;

            if (playerY == -currentMid) {
                LevelEvents.PlayerAdvanceChunk();
                _lastHeight = tilemapHeight;
                _lastMid = currentMid;
            }
        }
    }
}