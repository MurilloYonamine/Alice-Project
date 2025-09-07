namespace ENEMY {
    public abstract class EnemyState {
        public EnemyController _enemy;
        public virtual void OnEnter(EnemyController enemy) => _enemy = enemy;
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}