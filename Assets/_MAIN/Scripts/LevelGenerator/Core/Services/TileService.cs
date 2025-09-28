using UnityEngine.Tilemaps;
namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Services {
    public class TileService : ITileService {
        public void SetTile(Tilemap tilemap, TileBase tile, UnityEngine.Vector3Int position) {
            tilemap.SetTile(position, tile);
        }
    }
}
