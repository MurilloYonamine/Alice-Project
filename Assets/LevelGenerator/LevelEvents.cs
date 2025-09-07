using System;

namespace LEVELGENERATOR {
    public static class LevelEvents {
        public static event Action OnPlayerAdvanceChunk;

        public static void PlayerAdvanceChunk() {
            OnPlayerAdvanceChunk?.Invoke();
            LevelGeneratorManager.Instance.AdvanceToNextLevel();
        }
    }
}