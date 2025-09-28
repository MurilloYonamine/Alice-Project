using ALICE_PROJECT.PLAYER;
using ALICE_PROJECT.PLAYER.INPUT;
using UnityEngine;

namespace ALICE_PROJECT.ENEMY {
    public class EnemyController : MonoBehaviour {
        [Header("State Machine")]
        private EnemyState _currentState;
        private IdolState _idolState = new IdolState();
        private ChaseState _chaseState = new ChaseState();
        private PatrolState _patrolState = new PatrolState();

        [Header("Layers")]
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] public LayerMask _groundLayer;

        [SerializeField] public Transform _groundDetection;
        [SerializeField] private GameObject _weapon;

        private void Start() {
            ChangeState(_patrolState);
        }
        private void Update() {
            _currentState?.OnUpdate();
        }
        public void ChangeState(EnemyState newState) {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState?.OnEnter(this);
        }
        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player)) {
                DamageableObject damageableObject = player.GetComponent<DamageableObject>();
                if (damageableObject.Invencible) {
                    if (EnemyPoolManager.Instance == null) return;
                    EnemyPoolManager.Instance.ReturnToPool(gameObject);
                    InputEvents.RaisePlayerAttack();
                    
                    Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                    playerRb.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
                }
            }
        }
    }
}