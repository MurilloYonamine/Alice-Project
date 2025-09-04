using UnityEditor;
using UnityEngine;

namespace LEVEL_GENERATOR.EDITOR {
    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : Editor {

        #if UNITY_EDITOR
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            LevelData levelData = (LevelData)target;

            // Verifica se há dados em LevelData
            if (levelData.Squares == null || levelData.Squares.Count == 0) {
                EditorGUILayout.HelpBox("Nenhum dado de level encontrado. Crie o asset pelo window e salve para visualizar aqui.", MessageType.Info);
                return;
            }

            float width = 40;
            float height = 40;

            GUILayout.Space(10);
            GUILayout.Label("Level Preview", EditorStyles.boldLabel);

            // Desenha a grade de quadrados
            for (int x = 0; x < levelData.Squares.Count; x++) {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int y = 0; y < levelData.Squares[x].squares.Count; y++) {
                    Color choiceColor = GetColorForOption(levelData.Squares[x].squares[y]);
                    Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.Width(width), GUILayout.Height(height));
                    EditorGUI.DrawRect(rect, choiceColor);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        #endif
        private Color GetColorForOption(SquareStates option) {
            switch (option) {
                case SquareStates.Empty: return Color.gray;
                case SquareStates.Structure: return Color.yellow;
                case SquareStates.Enemy: return Color.red;
                default: return Color.white;
            }
        }
    }
}