using UnityEngine.Tilemaps;
namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Services {
    public interface ITileService {
        void SetTile(Tilemap tilemap, TileBase tile, UnityEngine.Vector3Int position);
    }
}
