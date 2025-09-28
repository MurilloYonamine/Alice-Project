using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using ALICE_PROJECT.ENEMY;
using ALICE_PROJECT.LEVELGENERATOR.UTILITIES;
using ALICE_PROJECT.LEVELGENERATOR.EDITOR.STRATEGY;
using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE;
using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Contexts;
using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE.Services;

namespace ALICE_PROJECT.LEVELGENERATOR {
    public class LevelGeneratorManager : MonoBehaviour {
        public static LevelGeneratorManager Instance { get; private set; }

        [Header("Tilemaps")]
        [field: SerializeField] public Tilemap GroundTilemap { get; private set; }
        [field: SerializeField] public Tilemap EmptyTilemap { get; private set; }
        private BoundsInt _bounds;

        [Header("Tiles")]
        [SerializeField] private TileBase _groundTile;
        [SerializeField] private TileBase _emptyTile;

        [Header("Levels")]
        [SerializeField] private List<LevelData> _levels;
        private int _currentRowIndex = 0;
        private int _currentLevelIndex = 0;
        private int _globalRowIndex = 0;
        public int LevelSize { get; private set; }

        [SerializeField] private GameObject _enemySpawnParent;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Start() {
            const string LEVEL_PATH = "Levels/";
            _levels = new List<LevelData>(Resources.LoadAll<LevelData>(LEVEL_PATH));
            if (_levels.Count == 0) {
                Debug.LogError("Nenhum LevelData encontrado em Resources/Levels!");
                return;
            }
            LevelSize = _levels[0].Cells.Count - 1;

            CountAndPoolEnemies();

            GroundTilemap.CompressBounds();
            EmptyTilemap.CompressBounds();
            _bounds = GroundTilemap.cellBounds;
        }

        private void CountAndPoolEnemies() {
            int enemyCount = 0;
            foreach (LevelData level in _levels) {
                foreach (LevelRow row in level.Cells) {
                    foreach (CellTypeSO cellType in row.rowElements) {
                        if (cellType is EnemyCellTypeSO) {
                            enemyCount++;
                        }
                    }
                }
            }
            if (EnemyPoolManager.Instance != null) {
                EnemyPoolManager.Instance.amountToPool = enemyCount;
                EnemyPoolManager.Instance.Initialize();
            }
        }

        public void AdvanceToNextLevel() {
            if (_currentLevelIndex >= _levels.Count) {
                Debug.LogWarning("Sem mais níveis para gerar.");
                return;
            }

            LevelData currentLevel = _levels[_currentLevelIndex];
            for (int i = 0; i < LevelSize; i++) {
                AdvanceToNextRow(currentLevel);
            }
            _currentLevelIndex++;
            _currentRowIndex = 0;
        }

        private void AdvanceToNextRow(LevelData level) {
            LevelRow currentRow = level.Cells[_currentRowIndex];
            int rowSize = currentRow.rowElements.Count;

            var tileService = new TileService();
            var enemySpawner = new EnemySpawner();

            for (int x = 0; x < rowSize; x++) {
                CellTypeSO cellType = currentRow.rowElements[x];
                Vector3Int coordinate = new Vector3Int(x, _globalRowIndex, 0);
                Vector3Int convertedCoordinate = CoordinateConverter.GetTileCoordinate(coordinate, _bounds);

                switch (cellType) {
                    case GroundCellTypeSO groundCell:
                        var groundCtx = new GroundCellContext {
                            tileService = tileService,
                            groundTilemap = GroundTilemap,
                            groundTile = _groundTile,
                            coordinate = convertedCoordinate
                        };
                        groundCell.Execute(groundCtx);
                        break;

                    case EmptyCellTypeSO emptyCell:
                        var emptyCtx = new EmptyCellContext {
                            tileService = tileService,
                            emptyTilemap = EmptyTilemap,
                            emptyTile = _emptyTile,
                            coordinate = convertedCoordinate
                        };
                        emptyCell.Execute(emptyCtx);
                        break;

                    case EnemyCellTypeSO enemyCell:
                        var enemyCtx = new EnemyCellContext {
                            enemySpawner = enemySpawner,
                            groundTilemap = GroundTilemap,
                            coordinate = convertedCoordinate,
                            enemyParent = _enemySpawnParent.transform
                        };
                        enemyCell.Execute(enemyCtx);
                        break;
                }
            }
            _currentRowIndex++;
            _globalRowIndex++;
        }
    }
}