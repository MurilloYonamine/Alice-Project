using System;
using UnityEngine;

namespace ALICE_PROJECT.PLAYER {
    [Serializable]
    public class PlayerJump {
        private readonly Rigidbody2D _rigidBody2D;
        private readonly LayerMask _groundLayer;

        [SerializeField] private float _jumpForce = 15;
        [SerializeField] private float _coyoteTime = 0.15f;
        [SerializeField] private float _jumpBufferTime = 0.15f;
        [SerializeField] private float _fallMultiplier = 2.5f;
        [SerializeField] private float _lowJumpMultiplier = 2f;

        private float _coyoteTimer;
        private float _jumpBufferCounter;

        [field: SerializeField] public bool IsGrounded { get; private set; }

        public PlayerJump(Rigidbody2D rigidBody2D, LayerMask groundLayer) {
            _rigidBody2D = rigidBody2D;
            _groundLayer = groundLayer;
            ConfigureRigidBody2D();
        }

        public void Jump() {
            _jumpBufferCounter = _jumpBufferTime;
        }

        public void Update() {
            // contadores de coyote e jump buffer
            if (_coyoteTimer > 0f) _coyoteTimer -= Time.deltaTime;
            if (_jumpBufferCounter > 0f) _jumpBufferCounter -= Time.deltaTime;

            // gravidade estilo Celeste
            if (_rigidBody2D.linearVelocity.y < 0f) {
                _rigidBody2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1f) * Time.deltaTime;
            } else if (_rigidBody2D.linearVelocity.y > 0f) {
                _rigidBody2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1f) * Time.deltaTime;
            }

            // executar jump se estiver dentro do coyote time ou jump buffer
            if ((_coyoteTimer > 0f || IsGrounded) && _jumpBufferCounter > 0f) {
                _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, _jumpForce);
                IsGrounded = false;
                _coyoteTimer = 0f;
                _jumpBufferCounter = 0f;
            }
        }

        public void CollisionEnter2D(Collision2D collision) {
            if (((1 << collision.gameObject.layer) & _groundLayer) != 0) {
                IsGrounded = true;
                _coyoteTimer = _coyoteTime;
            }
        }

        public void CollisionExit2D(Collision2D collision) {
            if (((1 << collision.gameObject.layer) & _groundLayer) != 0) {
                IsGrounded = false;
            }
        }

        // Configura o Rigidbody2D para valores próximos ao Celeste
        private void ConfigureRigidBody2D() {
            _rigidBody2D.gravityScale = 2.2f; // gravidade mais forte para saltos responsivos
            _rigidBody2D.mass = 0.8f; // massa leve para maior controle
            _rigidBody2D.linearDamping = 0f; // sem arrasto para não travar movimento
            _jumpForce = 18f; // força de pulo ajustada
            _fallMultiplier = 3.2f; // queda rápida
            _lowJumpMultiplier = 2.2f; // pulo baixo mais responsivo
        }
    }
}
