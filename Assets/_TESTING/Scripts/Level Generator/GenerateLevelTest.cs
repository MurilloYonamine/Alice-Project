using ALICE_PROJECT.LEVELGENERATOR.DATA;
using ALICE_PROJECT.LEVELGENERATOR.UTILITIES;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LEVELGENERATOR.TESTING {
    public class GenerateLevelTest : MonoBehaviour {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _tileTest;

        [field: SerializeField] public TileBase StructureTile { get; private set; }
        [field: SerializeField] public TileBase EnemyTile { get; private set; }
        [field: SerializeField] public TileBase EmptyTile { get; private set; }
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
                for (int y = 0; y < _levelData.Squares[x].rowElements.Count; y++) {
                    Vector3Int coordinate = new(y, x, 0);
                    switch (_levelData.Squares[x].rowElements[y]) {
                        case SquareStates.Ground:
                            _tilemap.SetTile(CoordinateConverter.GetTileCoordinate(coordinate, bounds), StructureTile);
                            break;
                        case SquareStates.Enemy:
                            _tilemap.SetTile(CoordinateConverter.GetTileCoordinate(coordinate, bounds), EnemyTile);
                            break;
                        case SquareStates.Empty:
                            _tilemap.SetTile(CoordinateConverter.GetTileCoordinate(coordinate, bounds), EmptyTile);
                            break;
                    }
                }
            }

        }
    }
}