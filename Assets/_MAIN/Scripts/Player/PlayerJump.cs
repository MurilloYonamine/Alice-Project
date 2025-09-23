using System;
using UnityEngine;

namespace PLAYER {
    /// <summary>
    /// Handles player jump mechanics including jump force, cut, fall speed, and ground detection.
    /// </summary>
    [Serializable]
    public class PlayerJump {
        private readonly Rigidbody2D _rigidBody2D;
        private readonly LayerMask _groundLayer;

        [Header("Jump Settings")]
        [Tooltip("Force applied when the player jumps.")]
        [SerializeField] private float _jumpForce = 16;

        [Tooltip("Multiplier applied to vertical velocity when jump is cut (button released early).")]
        [SerializeField] private float _jumpCutMultiplier = 0.5f;

        [Tooltip("Multiplier for gravity when the player is falling.")]
        [SerializeField] private float _fallMultiplier = 4f;

        [Tooltip("Multiplier for gravity when the jump button is released quickly.")]
        [SerializeField] private float _lowJumpMultiplier = 3f;

        [Tooltip("Maximum downward velocity allowed when falling.")]
        [SerializeField] private float _maxFallSpeed = 18f;

        public bool IsGrounded { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool JumpHeld { get; private set; }

        private DamageableObject _damageable;

        public PlayerJump(Rigidbody2D rigidBody2D, LayerMask groundLayer, DamageableObject damageable) {
            _rigidBody2D = rigidBody2D;
            _groundLayer = groundLayer;
            _damageable = damageable;

            _jumpForce = 16;
            _jumpCutMultiplier = 0.5f;
            _fallMultiplier = 4f;
            _lowJumpMultiplier = 3f;
            _maxFallSpeed = 18f;
        }

        public void OnUpdate() {
            ApplyJumpPhysics(IsGrounded);
        }

        public void HandleJumpPressed() {
            // Só registra tentativa de pulo se não estiver no knockback
            if (!_damageable.Invencible) {
                JumpPressed = true;
                JumpHeld = true;
            }
        }

        public void HandleJumpReleased() {
            JumpHeld = false;
        }

        public void ApplyJumpPhysics(bool isGrounded) {
            // Executa o pulo (impedido durante knockback/invencibilidade)
            if (JumpPressed && isGrounded && !_damageable.Invencible) {
                _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, _jumpForce);
                JumpPressed = false;
            }

            // Corta o pulo quando solta o botão
            if (!JumpHeld && _rigidBody2D.linearVelocity.y > 0) {
                _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x,
                    _rigidBody2D.linearVelocity.y * _jumpCutMultiplier);
            }

            // Aplica gravidade extra para queda mais rápida
            if (_rigidBody2D.linearVelocity.y < 0) {
                _rigidBody2D.linearVelocity += (_fallMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;

                // Limita a velocidade máxima de queda
                if (_rigidBody2D.linearVelocity.y < -_maxFallSpeed) {
                    _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, -_maxFallSpeed);
                }
            }
            // Pulo baixo quando solta rapidamente
            else if (_rigidBody2D.linearVelocity.y > 0 && !JumpHeld) {
                _rigidBody2D.linearVelocity += (_lowJumpMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
            }

            JumpPressed = false;
        }

        public void CollisionEnter2D(Collision2D collision2D) {
            if (((1 << collision2D.gameObject.layer) & _groundLayer) != 0) {
                IsGrounded = true;
                _rigidBody2D.linearDamping = 0f;
            }
        }

        public void CollisionExit2D(Collision2D collision2D) {
            IsGrounded = false;
            _rigidBody2D.linearDamping = 3f;
        }

        public void SetJumpForce(float jumpForce) => _jumpForce = jumpForce;
        public void SetJumpCutMultiplier(float jumpCutMultiplier) => _jumpCutMultiplier = jumpCutMultiplier;
        public void SetFallMultiplier(float fallMultiplier) => _fallMultiplier = fallMultiplier;
        public void SetLowJumpMultiplier(float lowJumpMultiplier) => _lowJumpMultiplier = lowJumpMultiplier;
        public void SetMaxFallSpeed(float maxFallSpeed) => _maxFallSpeed = maxFallSpeed;
    }
}
