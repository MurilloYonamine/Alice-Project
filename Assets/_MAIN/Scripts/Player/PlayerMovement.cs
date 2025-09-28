using UnityEngine;
using System;

namespace ALICE_PROJECT.PLAYER {
    [Serializable]
    public class PlayerMovement {
        private readonly Rigidbody2D _rigidBody2D;
        private float _speed = 7.5f; // valor arcade
        private Vector2 _movement;

        public PlayerMovement(Rigidbody2D rigidBody2D, float speed = 7.5f) {
            _rigidBody2D = rigidBody2D;
            _speed = speed;
        }

        public void SetMovement(Vector2 movement) {
            _movement = movement;
        }

        public void Update() {
            _rigidBody2D.linearVelocity = new Vector2(_movement.x * _speed, _rigidBody2D.linearVelocity.y);
        }
    }
}
