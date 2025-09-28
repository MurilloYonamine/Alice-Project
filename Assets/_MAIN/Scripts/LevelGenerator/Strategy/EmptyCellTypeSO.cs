using UnityEngine;
using UnityEngine.Tilemaps;

using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE;
using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Contexts;

namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.STRATEGY {
    [CreateAssetMenu(menuName = "LevelGenerator/CellTypes/Empty")]
    public class EmptyCellTypeSO : CellTypeSO {
        public override void Execute(ICellTypeContext context) {
            var ctx = context as EmptyCellContext;
            ctx.tileService.SetTile(ctx.emptyTilemap, ctx.emptyTile, ctx.coordinate);
        }
    }
}
