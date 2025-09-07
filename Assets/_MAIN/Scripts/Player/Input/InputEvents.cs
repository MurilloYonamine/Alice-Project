using System;
using PLAYER.INPUT;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PLAYER.INPUT {
    public static class InputEvents {
        public static event Action<Vector2> OnPlayerMove;
        public static event Action<Vector2> OnPlayerTouchMove;
        public static event Action OnPlayerAttack;
        public static event Action OnPlayerDeath;
        public static event Action OnPlayerJump;
        public static event Action OnPlayerTouchJump;
        public static event Action OnPlayerJumpReleased;

        public static void RaisePlayerMove(Vector2 direction) => OnPlayerMove?.Invoke(direction);
        public static void RaisePlayerTouchMove(Vector2 direction) => OnPlayerTouchMove?.Invoke(direction);
        public static void RaisePlayerDeath() => OnPlayerDeath?.Invoke();
        public static void RaisePlayerJump() => OnPlayerJump?.Invoke();
        public static void RaisePlayerTouchJump() => OnPlayerTouchJump?.Invoke();
        public static void RaisePlayerJumpReleased() => OnPlayerJumpReleased?.Invoke();
        public static void RaisePlayerAttack() => OnPlayerAttack?.Invoke();
    }
}