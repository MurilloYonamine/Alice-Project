using UnityEngine;
using PLAYER;
using PLAYER.INPUT;

namespace ENEMY {
    public class EnemyHitCollider : MonoBehaviour {
        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.TryGetComponent<PlayerController>(out var player)) {
                Debug.Log("Player hit by HitCollider!");
                GameObject parent = transform.parent.gameObject;
                EnemyPoolManager.Instance.ReturnToPool(parent);
                InputEvents.RaisePlayerAttack();
            }
        }
    }
}