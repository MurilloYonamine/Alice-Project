using PLAYER;
using UnityEngine;

namespace ENEMY {
    public class EnemyWeapon : MonoBehaviour {
        [SerializeField] private float _knockbackForce = 100f;
        [SerializeField] private float _damage = 1f;
        void OnCollisionEnter2D(Collision2D collision) {
            Collider2D collider2D = collision.collider;
            IDamageable damageable = collider2D.GetComponent<IDamageable>();

            if (damageable != null) {
                Vector2 direction = (collider2D.transform.position - transform.position).normalized;

                Vector2 knockback = direction * _knockbackForce;

                damageable.OnHit(_damage, knockback);
            }
        }
    
    }
}