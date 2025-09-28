using ALICE_PROJECT.ENEMY;
using UnityEngine;
namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Services {
    public class EnemySpawner : IEnemySpawner {
        public void SpawnEnemy(Vector3 worldPosition, Transform parent) {
            GameObject enemy = EnemyPoolManager.Instance.GetPooledObject();
            if (enemy != null) {
                enemy.transform.position = worldPosition;
                enemy.transform.SetParent(parent);
                enemy.SetActive(true);
            }
        }
    }
}
