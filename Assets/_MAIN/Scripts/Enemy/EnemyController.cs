using PLAYER;
using PLAYER.INPUT;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace ENEMY {
    public class EnemyController : MonoBehaviour {
        [Header("State Machine")]
        [SerializeField] private EnemyState _currentState;
        private IdolState _idolState = new IdolState();
        private PatrolState _patrolState = new PatrolState();
        private ChaseState _chaseState = new ChaseState();

        [Header("Layers")]
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] public LayerMask _groundLayer;

        [Header("Components")]
        [SerializeField] public BoxCollider2D HitCollider;
        [SerializeField] public Transform _groundDetection;
        [SerializeField] private BoxCollider2D _chaseArea;

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