using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Services;
using UnityEngine.Tilemaps;

namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Contexts {
    public class EmptyCellContext : ICellTypeContext {
        public ITileService tileService;
        public Tilemap emptyTilemap;
        public TileBase emptyTile;
        public UnityEngine.Vector3Int coordinate;
    }
}
