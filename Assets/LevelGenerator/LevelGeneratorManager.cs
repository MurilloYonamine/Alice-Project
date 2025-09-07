using System;
using System.Collections;
using System.Collections.Generic;
using ENEMY;
using LEVELGENERATOR.DATA;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LEVELGENERATOR {
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
            LevelSize = _levels[0].Squares.Count - 1;

            GetAllEnemies();

            GroundTilemap.CompressBounds();
            EmptyTilemap.CompressBounds();

            _bounds = GroundTilemap.cellBounds;
        }
        private void GetAllEnemies() {
            int enemyCount = 0;
            foreach (LevelData level in _levels) {
                foreach (SquareRow row in level.Squares) {
                    foreach (SquareStates state in row.rowElements) {
                        if (state == SquareStates.Enemy) {
                            enemyCount++;
                        }
                    }
                }
            }
            EnemyPoolManager.Instance.amountToPool = enemyCount;
            EnemyPoolManager.Instance.Initialize();
        }
        public void AdvanceToNextLevel() {
            if (_currentLevelIndex >= _levels.Count) {
                Debug.LogWarning("Sem mais nívels para gerar.");
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
            LevelData currentLevel = level;
            SquareRow currentRow = currentLevel.Squares[_currentRowIndex];
            int rowSize = currentRow.rowElements.Count;

            for (int x = 0; x < rowSize; x++) {
                SquareStates currentSquareState = currentRow.rowElements[x];
                Vector3Int coordinate = new Vector3Int(x, _globalRowIndex, 0);
                Vector3Int convertedCoordinate = CoordinateConverter.GetTileCoordinate(coordinate, _bounds);

                switch (currentSquareState) {
                    case SquareStates.Empty: EmptyTilemap.SetTile(convertedCoordinate, _emptyTile); break;
                    case SquareStates.Ground: GroundTilemap.SetTile(convertedCoordinate, _groundTile); break;
                    case SquareStates.Enemy:
                        EmptyTilemap.SetTile(convertedCoordinate, _emptyTile);
                        EnemyPoolManager enemyPool = EnemyPoolManager.Instance;
                        GameObject enemy = enemyPool.GetPooledObject();

                        enemy.SetActive(true);

                        float offset = 0.5f;
                        Vector3 enemyPosition = new Vector3(convertedCoordinate.x + offset, convertedCoordinate.y + offset, 0f);
                        enemy.transform.position = enemyPosition;

                        enemy.name = $"Enemy ({_globalRowIndex}, {x})";
                        break;
                    default: break;
                }
            }
            _currentRowIndex++;
            _globalRowIndex++;
        }
    }
}