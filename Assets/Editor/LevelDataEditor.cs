using LEVELGENERATOR.DATA;
using UnityEditor;
using UnityEngine;

namespace LEVELGENERATOR.EDITOR {
        #if UNITY_EDITOR
    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : Editor {

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            LevelData levelData = (LevelData)target;

            // Verifica se há dados em LevelData
            if (levelData.Squares == null || levelData.Squares.Count == 0) {
                EditorGUILayout.HelpBox("Nenhum dado de level encontrado. Crie o asset pelo window e salve para visualizar aqui.", MessageType.Info);
                return;
            }

            float width = 24;
            float height = 24;

            GUILayout.Space(10);
            GUILayout.Label("Level Preview", EditorStyles.boldLabel);

            // Desenha a grade de quadrados
            for (int x = 0; x < levelData.Squares.Count; x++) {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int y = 0; y < levelData.Squares[x].rowElements.Count; y++) {
                    Color choiceColor = GetColorForOption(levelData.Squares[x].rowElements[y]);
                    Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.Width(width), GUILayout.Height(height));
                    EditorGUI.DrawRect(rect, choiceColor);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        private Color GetColorForOption(SquareStates option) {
            switch (option) {
                case SquareStates.Empty: return Color.gray;
                case SquareStates.Ground: return Color.yellow;
                case SquareStates.Enemy: return Color.red;
                default: return Color.white;
            }
        }
    }
        #endif
}