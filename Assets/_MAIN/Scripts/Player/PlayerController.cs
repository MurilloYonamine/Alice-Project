using ALICE_PROJECT.LEVELGENERATOR;
using ALICE_PROJECT.PLAYER.INPUT;
using UnityEngine;

namespace ALICE_PROJECT.PLAYER {
    [RequireComponent(typeof(DamageableObject))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour {
        [Header("Components")]
        private DamageableObject _damageableObject;
        private Rigidbody2D _rigidBody2D;
        private PlayerInputHandler _playerInput;

        [Header("Layers")]
        [SerializeField] private LayerMask _groundLayer;

        [Header("Movement")]
        [SerializeField] private float _speed = 12f;
        private Vector2 _movement;

        [Header("Player Components")]
        [field: SerializeField] public PlayerJump PlayerJump { get; private set; }
        [field: SerializeField] public PlayerSmash PlayerSmash { get; private set; }
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
        [field: SerializeField] public PlayerLevelTracker PlayerLevelTracker { get; private set; }

        private void Awake() {
            _playerInput = new PlayerInputHandler();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _damageableObject = GetComponent<DamageableObject>();

            PlayerJump = new PlayerJump(_rigidBody2D, _groundLayer);
            PlayerSmash = new PlayerSmash(_rigidBody2D, _damageableObject, PlayerJump);
            PlayerMovement = new PlayerMovement(_rigidBody2D, _speed);
            PlayerLevelTracker = new PlayerLevelTracker(transform);
        }

        private void Start() {
            if (LevelGeneratorManager.Instance == null) return;
            PlayerLevelTracker = new PlayerLevelTracker(transform);
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
            PlayerMovement.Update();
            PlayerJump.Update();
            PlayerLevelTracker.Update();
        }

        private void HandleMoveInput(Vector2 inputDirection) {
            PlayerMovement.SetMovement(inputDirection);
        }

        private void HandlePlayerJump() {
            PlayerJump.Jump();
        }

        private void HandlePlayerSmashDown() {
            PlayerSmash.SmashDown();
        }
        private void OnCollisionEnter2D(Collision2D collision) {
            PlayerJump.CollisionEnter2D(collision);
            PlayerSmash.OnCollisionEnter2D();
        }

        private void OnCollisionExit2D(Collision2D collision) {
            PlayerJump.CollisionExit2D(collision);
        }

        private void OnDestroy() {
            _playerInput?.Dispose();
        }
    }
}
