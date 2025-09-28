using System;
using UnityEngine;

namespace ALICE_PROJECT.PLAYER {
    /// <summary>
    /// Classe simples para pulo do jogador.
    /// </summary>
    [Serializable]
    public class PlayerJump {
        private readonly Rigidbody2D _rigidBody2D;
        private readonly LayerMask _groundLayer;
        [SerializeField] private float _jumpForce = 16f;
        [field: SerializeField] public bool IsGrounded { get; private set; }

        public PlayerJump(Rigidbody2D rigidBody2D, LayerMask groundLayer) {
            _rigidBody2D = rigidBody2D;
            _groundLayer = groundLayer;
        }

        public void Jump() {
            if (IsGrounded) {
                _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, _jumpForce);
                IsGrounded = false;
            }
        }

        public void CollisionEnter2D(Collision2D collision2D) {
            if (((1 << collision2D.gameObject.layer) & _groundLayer) != 0) {
                IsGrounded = true;
            }
        }

        public void CollisionExit2D(Collision2D collision2D) {
            IsGrounded = false;
        }
    }
}
