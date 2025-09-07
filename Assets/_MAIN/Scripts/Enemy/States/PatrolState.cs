using UnityEngine;
namespace ENEMY {
    public class PatrolState : EnemyState {
        private int patrolDistance;
        private BoxCollider2D _boxCollider2D;
        private float _enemySpeed;
        public override void OnEnter(EnemyController enemy) {
            base.OnEnter(enemy);
        }
        public override void OnUpdate() {
        }
        public override void OnExit() {
        }
    }
}