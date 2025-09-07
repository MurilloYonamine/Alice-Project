using System;
using System.Collections;
using System.Collections.Generic;
using LEVELGENERATOR.DATA;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LEVELGENERATOR {
    public class LevelGeneratorManager : MonoBehaviour {
        public static LevelGeneratorManager Instance { get; private set; }

        [Header("Tilemaps")]
        [field: SerializeField] public Tilemap StructureTilemap { get; private set; }
        [field: SerializeField] public Tilemap EmptyTilemap { get; private set; }
        [field: SerializeField] public Tilemap EnemyTilemap { get; private set; }
        private BoundsInt _bounds;

        [Header("Tiles")]
        [SerializeField] private TileBase _structureTile;
        [SerializeField] private TileBase _enemyTile;
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
        private void Start() {
            const string LEVEL_PATH = "Levels/";

            _levels = new List<LevelData>(Resources.LoadAll<LevelData>(LEVEL_PATH));
            LevelSize = _levels[0].Squares.Count - 1;

            StructureTilemap.CompressBounds();
            EmptyTilemap.CompressBounds();
            EnemyTilemap.CompressBounds();

            _bounds = StructureTilemap.cellBounds;

            AdvanceToNextLevel();
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
                    case SquareStates.Structure: StructureTilemap.SetTile(convertedCoordinate, _structureTile); break;
                    case SquareStates.Enemy: EnemyTilemap.SetTile(convertedCoordinate, _enemyTile); break;
                    default: break;
                }
            }
            _currentRowIndex++;
            _globalRowIndex++;
        }
    }
}