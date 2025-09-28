using System.Collections;
using ALICE_PROJECT.INTERFACES;
using ALICE_PROJECT.PLAYER;
using UnityEngine;

namespace ALICE_PROJECT {
    [RequireComponent(typeof(Rigidbody2D))]
    public class DamageableObject : MonoBehaviour, IDamageable {
        private Rigidbody2D _rigidBody2D;

        [SerializeField] private float _health;
        [SerializeField] private bool _targetable;
        [SerializeField] private bool _invencible;
        [SerializeField] private bool _disableSimulation;
        [SerializeField] public Collider2D _physicsCollider;

        public float Health {
            get => _health;
            set {
                _health = value;
                if (_health <= 0) {
                    Targetable = false;
                }
            }
        }
        public bool Targetable {
            get => _targetable; set {
                if (_disableSimulation) {
                    _rigidBody2D.simulated = false;
                }
                _physicsCollider.enabled = value;
            }
        }
        public bool Invencible {
            get => _invencible; set {
                _invencible = value;
            }
        }

        private void Awake() {
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }

        public void OnHit(float damage, Vector2 knockback) {
            if (!Invencible) {
                Health -= damage;

                //_rigidBody2D.AddForce(knockback, ForceMode2D.Impulse);

                // Invencible = true;
                // _physicsCollider.enabled = !Invencible;
            }
        }

        public void OnHit(float damage) {
            if (!Invencible) {
                Health -= damage;

                Invencible = true;
            }
        }

        public void OnObjectDestroyed() {
            throw new System.NotImplementedException();
        }
    }
}