using LEVELGENERATOR;
using PLAYER.INPUT;
using TMPro;
using UnityEngine;

namespace PLAYER {
    public class PlayerController : MonoBehaviour {
        private Rigidbody2D _rigidBody2D;
        private PlayerInputHandler _playerInput;
        [SerializeField] private PlayerJump _playerJump;

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

        [SerializeField] private float knockbackForce = 10f;

        [Header("Score Test")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        private int score;
        private void Start() {
            _playerInput = new PlayerInputHandler();
            _rigidBody2D = GetComponent<Rigidbody2D>();

            // Inicializa o sistema de pulo
            _playerJump = new PlayerJump(_rigidBody2D, _groundLayer);

            _levelSize = LevelGeneratorManager.Instance.LevelSize;
            LevelGeneratorManager.Instance.AdvanceToNextLevel();
        }
        private void OnEnable() {
            InputEvents.OnPlayerMove += HandleMoveInput;
            InputEvents.OnPlayerTouchMove += HandleTouchMoveInput;
            InputEvents.OnPlayerJump += HandlePlayerJump;
            InputEvents.OnPlayerTouchJump += HandlePlayerTouchJump;
            InputEvents.OnPlayerJumpReleased += HandlePlayerJumpReleased;
            InputEvents.OnPlayerAttack += Attack;
        }
        private void OnDisable() {
            InputEvents.OnPlayerMove -= HandleMoveInput;
            InputEvents.OnPlayerTouchMove -= HandleTouchMoveInput;
            InputEvents.OnPlayerJump -= HandlePlayerJump;
            InputEvents.OnPlayerTouchJump -= HandlePlayerTouchJump;
            InputEvents.OnPlayerJumpReleased -= HandlePlayerJumpReleased;
            InputEvents.OnPlayerAttack -= Attack;
        }
        private void Update() {
            GetCurrentLocation();
            _playerJump.OnUpdate();
            _rigidBody2D.linearVelocity = new Vector2(_movement.x * _speed, _rigidBody2D.linearVelocity.y);
        }
        private void HandleMoveInput(Vector2 inputDirection) => _movement = inputDirection;
        
        private void HandleTouchMoveInput(Vector2 inputDirection) {
            // Para mobile, usamos apenas o eixo X (horizontal) do delta de movimento
            _movement = new Vector2(inputDirection.x, 0);
        }
        
        private void HandlePlayerJump() {
            _playerJump.HandleJumpPressed();
        }
        
        private void HandlePlayerTouchJump() {
            _playerJump.HandleJumpPressed();
        }
        
        private void HandlePlayerJumpReleased() {
            _playerJump.HandleJumpReleased();
        }
        private void OnDestroy() {
            _playerInput?.Dispose();
        }
        private bool _hasTriggeredAdvance = false;
        private void Attack() {
            score++;
            Debug.Log("Score: " + score);
            ApplyKnockback(Vector2.up);
            _scoreText.text = score.ToString();
        }
        public void ApplyKnockback(Vector2 direction) {
            _rigidBody2D.linearVelocity = Vector2.zero; // reseta a velocidade atual
            _rigidBody2D.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
        private void GetCurrentLocation() {
            // Pega a altura da tilemap e a posição do player
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
            _playerJump.CollisionEnter2D(collision2D);
        }
        private void OnCollisionExit2D(Collision2D collision2D) {
            _playerJump.CollisionExit2D(collision2D);
        }
    }
}