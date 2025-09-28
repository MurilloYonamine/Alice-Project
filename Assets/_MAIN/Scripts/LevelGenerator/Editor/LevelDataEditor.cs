using ALICE_PROJECT.LEVELGENERATOR.DATA;
using UnityEditor;
using UnityEngine;

namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR {
#if UNITY_EDITOR
    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : Editor {

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            LevelData levelData = (LevelData)target;

            // Verifica se há dados em LevelData
            if (levelData.Cells == null || levelData.Cells.Count == 0) {
                EditorGUILayout.HelpBox("Nenhum dado de level encontrado. Crie o asset pelo window e salve para visualizar aqui.", MessageType.Info);
                return;
            }

            float width = 24;
            float height = 24;

            GUILayout.Space(10);
            GUILayout.Label("Level Preview", EditorStyles.boldLabel);

            // Desenha a grade de células
            for (int x = 0; x < levelData.Cells.Count; x++) {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int y = 0; y < levelData.Cells[x].rowElements.Count; y++) {
                    Color choiceColor = GetColorForOption(levelData.Cells[x].rowElements[y]);
                    Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.Width(width), GUILayout.Height(height));
                    EditorGUI.DrawRect(rect, choiceColor);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        private Color GetColorForOption(CellType option) {
            switch (option) {
                case CellType.Empty: return Color.gray;
                case CellType.Ground: return Color.yellow;
                case CellType.Enemy: return Color.red;
                default: return Color.white;
            }
        }
    }
#endif
}