using UnityEngine;
using UnityEngine.Tilemaps;

using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE;
using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Contexts;

namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.STRATEGY {
    [CreateAssetMenu(menuName = "LevelGenerator/CellTypes/Enemy")]
    public class EnemyCellTypeSO : CellTypeSO {
        public override void Execute(ICellTypeContext context) {
            var ctx = context as EnemyCellContext;
            ctx.enemySpawner.SpawnEnemy(ctx.groundTilemap.CellToWorld(ctx.coordinate), ctx.enemyParent);
        }
    }
}
