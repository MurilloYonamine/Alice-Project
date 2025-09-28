using UnityEngine;
using UnityEngine.Tilemaps;
using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE;
using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Contexts;

namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.STRATEGY {
    [CreateAssetMenu(menuName = "LevelGenerator/CellTypes/Ground")]
    public class GroundCellTypeSO : CellTypeSO {
        public override void Execute(ICellTypeContext context) {
            var ctx = context as GroundCellContext;
            ctx.tileService.SetTile(ctx.groundTilemap, ctx.groundTile, ctx.coordinate);
        }
    }
}
