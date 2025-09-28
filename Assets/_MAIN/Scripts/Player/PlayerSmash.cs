using UnityEngine;
using System;
namespace ALICE_PROJECT.PLAYER {
    [Serializable]
    public class PlayerSmash {
        private readonly Rigidbody2D _rigidBody2D;
        private readonly DamageableObject _damageableObject;
        private readonly PlayerJump _playerJump;
        private bool _isSmashing = false;
        private float _smashForce = -60f; // mais rápido
        private float _smashBounceForce = 10f; // rebote arcade

        public bool IsSmashing => _isSmashing;

        public PlayerSmash(Rigidbody2D rigidBody2D, DamageableObject damageableObject, PlayerJump playerJump) {
            _rigidBody2D = rigidBody2D;
            _damageableObject = damageableObject;
            _playerJump = playerJump;
        }

        public void SmashDown() {
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, _smashForce);
            _isSmashing = true;
            _damageableObject.Invencible = true;
        }

        public void OnCollisionEnter2D() {
            if (_isSmashing && _playerJump.IsGrounded) {
                _isSmashing = false;
                _damageableObject.Invencible = false;
                _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, _smashBounceForce);
            }
        }
    }
}
