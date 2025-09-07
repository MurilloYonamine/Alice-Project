using UnityEngine;

namespace PLAYER {
    [System.Serializable]
    public class PlayerJump {
        private readonly Rigidbody2D _rigidBody2D;
        private readonly LayerMask _groundLayer;
        
        [Header("Jump Settings")]
        [SerializeField] private float _jumpForce = 12f;
        [SerializeField] private float _jumpCutMultiplier = 0.5f;
        [SerializeField] private float _fallMultiplier = 4f;
        [SerializeField] private float _lowJumpMultiplier = 3f;
        [SerializeField] private float _maxFallSpeed = 18f;

        private bool _isGrounded;
        private bool _jumpPressed;
        private bool _jumpHeld;

        public PlayerJump(Rigidbody2D rigidBody2D, LayerMask groundLayer) {
            _rigidBody2D = rigidBody2D;
            _groundLayer = groundLayer;

            _jumpForce = 12f;
            _jumpCutMultiplier = 0.5f;
            _fallMultiplier = 4f;
            _lowJumpMultiplier = 3f;
            _maxFallSpeed = 18f;
        }

        public void OnUpdate() {
            ApplyJumpPhysics(_isGrounded);
        }

        public void HandleJumpPressed() {
            _jumpPressed = true;
            _jumpHeld = true;
        }

        public void HandleJumpReleased() {
            _jumpHeld = false;
        }

        public void ApplyJumpPhysics(bool isGrounded) {
            // Executa o pulo
            if (_jumpPressed && isGrounded) {
                _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, _jumpForce);
                _jumpPressed = false;
            }

            // Corta o pulo quando solta o botão
            if (!_jumpHeld && _rigidBody2D.linearVelocity.y > 0) {
                _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x,
                    _rigidBody2D.linearVelocity.y * _jumpCutMultiplier);
            }

            // Aplica gravidade extra para queda mais rápida
            if (_rigidBody2D.linearVelocity.y < 0) {
                _rigidBody2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;

                // Limita a velocidade máxima de queda
                if (_rigidBody2D.linearVelocity.y < -_maxFallSpeed) {
                    _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, -_maxFallSpeed);
                }
            }
            // Pulo baixo quando solta rapidamente
            else if (_rigidBody2D.linearVelocity.y > 0 && !_jumpHeld) {
                _rigidBody2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
            }

            _jumpPressed = false;
        }

        public void CollisionEnter2D(Collision2D collision2D) {
            if (((1 << collision2D.gameObject.layer) & _groundLayer) != 0) {
                _isGrounded = true;
                Debug.Log("No Chão");
            }
        }
        public void CollisionExit2D(Collision2D collision2D) {
            _isGrounded = false;
            Debug.Log("No Ar");
        }

        public void SetJumpForce(float jumpForce) => _jumpForce = jumpForce;
        public void SetJumpCutMultiplier(float jumpCutMultiplier) => _jumpCutMultiplier = jumpCutMultiplier;
        public void SetFallMultiplier(float fallMultiplier) => _fallMultiplier = fallMultiplier;
        public void SetLowJumpMultiplier(float lowJumpMultiplier) => _lowJumpMultiplier = lowJumpMultiplier;
        public void SetMaxFallSpeed(float maxFallSpeed) => _maxFallSpeed = maxFallSpeed;
    }
}
