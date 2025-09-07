using UnityEngine;

namespace ENEMY {
    public class EnemyController : MonoBehaviour {
        [Header("State Machine")]
        private EnemyState _currentState;
        private IdolState _idolState = new IdolState();
        private PatrolState _patrolState = new PatrolState();
        private ChaseState _chaseState = new ChaseState();

        [Header("Layers")]
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private LayerMask _groundLayer;

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
    }
}