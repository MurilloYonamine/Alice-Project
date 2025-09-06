using System;
using System.Collections;
using System.Collections.Generic;
using LEVELGENERATOR.DATA;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LEVELGENERATOR {
    public class LevelGeneratorManager : MonoBehaviour {
        [Header("Tilemap")]
        [SerializeField] private Tilemap _tilemap;
        private BoundsInt _bounds;

        [Header("Tiles")]
        [SerializeField] private TileBase _structureTile;
        [SerializeField] private TileBase _enemyTile;
        [SerializeField] private TileBase _emptyTile;

        [Header("Levels")]
        [SerializeField] private List<LevelData> _levels;

        public static event Action OnPlayerAdvanceChunk;

        void Start() {
            const string LEVEL_PATH = "Levels/";

            _levels = new List<LevelData>(Resources.LoadAll<LevelData>(LEVEL_PATH));
            _tilemap.CompressBounds();
            _bounds = _tilemap.cellBounds;

            GetNextRow();
        }

        private int _currentRowIndex = 0;

        private void GetNextRow() {
            LevelData currentLevel = _levels[_currentRowIndex];
            SquareRow currentRow = currentLevel.Squares[_currentRowIndex];
            int rowSize = currentRow.rowElements.Count - 1;

            for (int x = 0; x < rowSize; x++) {
                SquareStates currentSquareState = currentRow.rowElements[x];
                Vector3Int coordinate = new Vector3Int(x, _currentRowIndex, 0);
                Vector3Int convertedCoordinate = CoordinateConverter.GetTileCoordinate(coordinate, _bounds);

                switch (currentSquareState) {
                    case SquareStates.Empty: _tilemap.SetTile(convertedCoordinate, _emptyTile); break;
                    case SquareStates.Structure: _tilemap.SetTile(convertedCoordinate, _structureTile); break;
                    case SquareStates.Enemy: _tilemap.SetTile(convertedCoordinate, _enemyTile); break;
                    default: break;
                }
            }
            _currentRowIndex++;
            OnPlayerAdvanceChunk?.Invoke();
        }
    }
}