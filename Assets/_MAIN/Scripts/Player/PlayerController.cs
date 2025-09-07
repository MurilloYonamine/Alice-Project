
using LEVELGENERATOR;
using PLAYER.INPUT;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PLAYER {
    public class PlayerController : MonoBehaviour {
        private Rigidbody2D _rigidBody2D;
        private PlayerInputHandler _playerInput;
        [SerializeField] private PlayerJump _playerJump;

        [Header("Layers")]
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private LayerMask _groundLayer;

        [Header("Movement")]
        [SerializeField] private Vector3 _movement;
        [SerializeField] private float _speed = 5f;

        [Header("Generation Control")]
        private int _lastHeight;
        private int _lastMid;
        private int _levelSize;

        private void Start() {
            _playerInput = new PlayerInputHandler();
            _rigidBody2D = GetComponent<Rigidbody2D>();

            // Inicializa o sistema de pulo
            _playerJump = new PlayerJump(_rigidBody2D, _groundLayer);

            _levelSize = LevelGeneratorManager.Instance.LevelSize;
        }
        private void OnEnable() {
            InputEvents.OnPlayerMove += HandleMoveInput;
            InputEvents.OnPlayerJump += HandlePlayerJump;
            InputEvents.OnPlayerJumpReleased += HandlePlayerJumpReleased;
        }
        private void OnDisable() {
            InputEvents.OnPlayerMove -= HandleMoveInput;
            InputEvents.OnPlayerJump -= HandlePlayerJump;
            InputEvents.OnPlayerJumpReleased -= HandlePlayerJumpReleased;
        }
        private void Update() {
            GetCurrentLocation();
            _playerJump.OnUpdate();
            _rigidBody2D.linearVelocity = new Vector2(_movement.x * _speed, _rigidBody2D.linearVelocity.y);
        }
        private void HandleMoveInput(Vector2 inputDirection) => _movement = inputDirection;
        private void HandlePlayerJump() {
            _playerJump.HandleJumpPressed();
        }
        private void HandlePlayerJumpReleased() {
            _playerJump.HandleJumpReleased();
        }
        private void OnDestroy() {
            _playerInput?.Dispose();
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
        private void OnCollisionEnter2D(Collision2D collision2D) {
            _playerJump.CollisionEnter2D(collision2D);
        }
        private void OnCollisionExit2D(Collision2D collision2D) {
            _playerJump.CollisionExit2D(collision2D);
        }
    }
}