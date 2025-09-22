using LEVELGENERATOR;
using PLAYER.INPUT;
using UnityEngine;

namespace PLAYER {
    public class PlayerController : MonoBehaviour {
        private Rigidbody2D _rigidBody2D;
        private PlayerInputHandler _playerInput;
        private TouchScreenMovement _touchScreenMovement;
        public PlayerJump PlayerJump { get; private set; }

        [Header("Layers")]
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private LayerMask _groundLayer;

        [Header("Movement")]
        [SerializeField] private Vector2 _movement;
        [SerializeField] private float _speed = 5f;

        [Header("Generation Control")]
        private int _lastHeight;
        private int _lastMid;
        private int _levelSize;

        private void Start() {
            _playerInput = new PlayerInputHandler();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            DamageableObject damageable = GetComponent<DamageableObject>();

            // Inicializa o sistema de pulo
            PlayerJump = new PlayerJump(_rigidBody2D, _groundLayer, damageable);

            if (LevelGeneratorManager.Instance == null) return;
            _levelSize = LevelGeneratorManager.Instance.LevelSize;

            LevelEvents.PlayerAdvanceChunk();
        }
        private void OnEnable() {
            InputEvents.OnPlayerMove += HandleMoveInput;
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
            PlayerJump.OnUpdate();
            _rigidBody2D.linearVelocity = new Vector2(_movement.x * _speed, _rigidBody2D.linearVelocity.y);
        }
        private void HandleMoveInput(Vector2 inputDirection) {
            _movement = inputDirection; 
        }
        private void HandlePlayerJump() {
            PlayerJump.HandleJumpPressed();
        }
        private void HandlePlayerJumpReleased() {
            PlayerJump.HandleJumpReleased();
        }
        private void OnDestroy() {
            _playerInput?.Dispose();
        }
        private bool _hasTriggeredAdvance = false;

        private void GetCurrentLocation() {
            // Pega a altura da tilemap e a posição do player

            if (LevelGeneratorManager.Instance == null) return;

            int tilemapHeight = LevelGeneratorManager.Instance.EmptyTilemap.size.y;
            Vector3Int playerWorldPosition = Vector3Int.RoundToInt(transform.position);
            Vector3Int playerCellPosition = LevelGeneratorManager.Instance.EmptyTilemap.WorldToCell(playerWorldPosition);

            // Centraliza a posição do player na tilemap
            int playerY = playerCellPosition.y - 9;
            int currentMid = tilemapHeight == _levelSize ? tilemapHeight / 2 : _levelSize + _lastMid;

            // Checa se passou da metade e ainda não avançou
            if (!_hasTriggeredAdvance && playerY <= -currentMid) {
                _hasTriggeredAdvance = true; // trava para não chamar várias vezes
                LevelEvents.PlayerAdvanceChunk();
                _lastHeight = tilemapHeight;
                _lastMid = currentMid;
            }

            // Reset quando subir para o próximo nível
            if (playerY < -_lastHeight) {
                _hasTriggeredAdvance = false;
            }
        }
        private void OnCollisionEnter2D(Collision2D collision2D) {
            PlayerJump.CollisionEnter2D(collision2D);
        }
        private void OnCollisionExit2D(Collision2D collision2D) {
            PlayerJump.CollisionExit2D(collision2D);
        }
    }
}