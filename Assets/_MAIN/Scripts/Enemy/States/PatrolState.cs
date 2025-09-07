using UnityEngine;
namespace ENEMY {
    public class PatrolState : EnemyState {
        private float _enemySpeed = 5f;
        public override void OnEnter(EnemyController enemy) {
            base.OnEnter(enemy);
            _enemySpeed = 5f;
        }
        public override void OnUpdate() {
            _enemy.transform.Translate(Vector2.right * (_enemySpeed * Time.deltaTime));

            RaycastHit2D groundInfo = Physics2D.Raycast(_enemy._groundDetection.position, Vector2.down, 2f, _enemy._groundLayer);

            if (groundInfo == false) {
                if (_enemy.transform.eulerAngles.y == 0) {
                    _enemy.transform.eulerAngles = new Vector3(0, -180, 0);
                }
                else {
                    _enemy.transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }

        }
        public override void OnExit() {
        }
    }
}