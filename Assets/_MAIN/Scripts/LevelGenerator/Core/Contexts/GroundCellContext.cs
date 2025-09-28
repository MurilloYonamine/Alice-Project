using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Services;
using UnityEngine.Tilemaps;

namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Contexts {
    public class GroundCellContext : ICellTypeContext {
        public ITileService tileService;
        public Tilemap groundTilemap;
        public TileBase groundTile;
        public UnityEngine.Vector3Int coordinate;
    }
}
