using UnityEngine;
namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Services {
    public interface IEnemySpawner {
        void SpawnEnemy(Vector3 worldPosition, Transform parent);
    }
}
