using PLAYER;
using PLAYER.INPUT;
using UnityEngine;

namespace ENEMY {
    public class HitArea : MonoBehaviour {
        [SerializeField] private GameObject originalGameObject;
        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player)) {

                if (EnemyPoolManager.Instance == null) return;
                Debug.Log("Player hit Enemy!");
                EnemyPoolManager.Instance.ReturnToPool(originalGameObject);
                InputEvents.RaisePlayerAttack();
            }
        }
    }
}