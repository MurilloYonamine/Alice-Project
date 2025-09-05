using UnityEngine;
using UnityEngine.Tilemaps;

namespace LEVEL_GENERATOR.TESTING {
    public class GenerateLevelTest : MonoBehaviour {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _tileTest;
        private void Start() {
            if (_levelData == null) {
                Debug.LogError("Esqueceu o LevelData.");
                return;
            }

            _tilemap.SetTile(new Vector3Int(0, 0, 0), _tileTest);

            _tilemap.ClearAllTiles();
            _tilemap.CompressBounds();

            BoundsInt bounds = _tilemap.cellBounds;

            for (int x = 0; x < _levelData.Squares.Count; x++) {
                for (int y = 0; y < _levelData.Squares[x].squares.Count; y++) {
                    Vector3Int coordinate = new(y, x, 0);
                    switch (_levelData.Squares[x].squares[y]) {
                        case SquareStates.Structure:
                            _tilemap.SetTile(CoordinateConverter.GetTileCoordinate(coordinate, bounds), _levelData.StructureTile);
                            break;
                        case SquareStates.Enemy:
                            _tilemap.SetTile(CoordinateConverter.GetTileCoordinate(coordinate, bounds), _levelData.EnemyTile);
                            break;
                        case SquareStates.Empty:
                            _tilemap.SetTile(CoordinateConverter.GetTileCoordinate(coordinate, bounds), _levelData.EmptyTile);
                            break;
                    }
                }
            }

        }
    }
}
public static class CoordinateConverter {
    public static Vector3Int GetTileCoordinate(Vector3Int cell, BoundsInt bounds) {
        int col = cell.x - bounds.xMin - 5;
        int row = bounds.yMax - cell.y + 10;

        return new Vector3Int(col, row, 0);
    }
}