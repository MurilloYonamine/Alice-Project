using ALICE_PROJECT.LEVELGENERATOR;
using ALICE_PROJECT.PLAYER.INPUT;
using ALICE_PROJECT.SCREEN;
using UnityEngine;

namespace ALICE_PROJECT.PLAYER {
    [RequireComponent(typeof(DamageableObject))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour {
        [Header("Components")]
    private DamageableObject _damageableObject;
    private Rigidbody2D _rigidBody2D;
    private PlayerInputHandler _playerInput;
    private TouchScreenMovement _touchScreenMovement;
    [field: SerializeField] public PlayerJump PlayerJump { get; private set; }
    [field: SerializeField] public PlayerSmash PlayerSmash { get; private set; }

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

        private void Awake() {
            _playerInput = new PlayerInputHandler();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _damageableObject = GetComponent<DamageableObject>();
            PlayerJump = new PlayerJump(_rigidBody2D, _groundLayer);
            PlayerSmash = new PlayerSmash(_rigidBody2D, _damageableObject, PlayerJump);
        }
        private void Start() {
            if (LevelGeneratorManager.Instance == null) return;
            _levelSize = LevelGeneratorManager.Instance.LevelSize;

            LevelEvents.PlayerAdvanceChunk();
        }
        private void OnEnable() {
            InputEvents.OnPlayerMove += HandleMoveInput;
            InputEvents.OnPlayerJump += HandlePlayerJump;
            InputEvents.OnPlayerSmashDown += HandlePlayerSmashDown;
        }
        private void OnDisable() {
            InputEvents.OnPlayerMove -= HandleMoveInput;
            InputEvents.OnPlayerJump -= HandlePlayerJump;
            InputEvents.OnPlayerSmashDown -= HandlePlayerSmashDown;
        }
        private void Update() {
            GetCurrentLocation();
            _rigidBody2D.linearVelocity = new Vector2(_movement.x * _speed, _rigidBody2D.linearVelocity.y);
        }
        private void HandleMoveInput(Vector2 inputDirection) {
            _movement = inputDirection;
        }
        private void HandlePlayerJump() {
            PlayerJump.Jump();
        }
        private void HandlePlayerSmashDown() {
            PlayerSmash.SmashDown();
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
            PlayerSmash.OnCollisionEnter2D();
        }
        private void OnCollisionExit2D(Collision2D collision2D) {
            PlayerJump.CollisionExit2D(collision2D);
        }
    }
}