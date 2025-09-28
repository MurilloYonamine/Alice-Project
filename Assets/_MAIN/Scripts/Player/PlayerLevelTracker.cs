using UnityEngine;
using System;
using ALICE_PROJECT.LEVELGENERATOR;

namespace ALICE_PROJECT.PLAYER {
    [Serializable]
    public class PlayerLevelTracker {
        private int _lastHeight;
        private int _lastMid;
        private int _levelSize;
        private bool _hasTriggeredAdvance = false;
        private Transform _playerTransform;

        public PlayerLevelTracker(Transform playerTransform) {
            _playerTransform = playerTransform;
            _levelSize = LevelGeneratorManager.Instance.LevelSize;
        }

        public void Update() {
            if (LevelGeneratorManager.Instance == null) return;

            int tilemapHeight = LevelGeneratorManager.Instance.EmptyTilemap.size.y;
            Vector3Int playerWorldPosition = Vector3Int.RoundToInt(_playerTransform.position);
            Vector3Int playerCellPosition = LevelGeneratorManager.Instance.EmptyTilemap.WorldToCell(playerWorldPosition);

            int playerY = playerCellPosition.y - 9;
            int currentMid = tilemapHeight == _levelSize ? tilemapHeight / 2 : _levelSize + _lastMid;

            if (!_hasTriggeredAdvance && playerY <= -currentMid) {
                _hasTriggeredAdvance = true;
                LevelEvents.PlayerAdvanceChunk();
                _lastHeight = tilemapHeight;
                _lastMid = currentMid;
            }

            if (playerY < -_lastHeight) {
                _hasTriggeredAdvance = false;
            }
        }
    }
}
