using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Services;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Contexts {
    public class EnemyCellContext : ICellTypeContext {
        public IEnemySpawner enemySpawner;
        public Tilemap groundTilemap;
        public Vector3Int coordinate;
        public Transform enemyParent;
    }
}
