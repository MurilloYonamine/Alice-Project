using UnityEditor;
using UnityEngine;


namespace LEVEL {
    [CustomEditor(typeof(LevelGenerator))]

    public class LevelEditor : Editor {
        SerializedProperty tilemap;
        SerializedProperty row;
        SerializedProperty col;

        private Texture square;

        private void OnEnable() {
            tilemap = serializedObject.FindProperty("_tilemap");
            row = serializedObject.FindProperty("_row");
            col = serializedObject.FindProperty("_col");
        }

#if UNITY_EDITOR
        public override void OnInspectorGUI() {
            serializedObject.Update();

            LevelGenerator levelGenerator = (LevelGenerator)target;

            // Exibe as propriedades no Inspetor
            EditorGUILayout.PropertyField(tilemap, new GUIContent("Tilemap"));
            EditorGUILayout.PropertyField(row, new GUIContent("Row"));
            EditorGUILayout.PropertyField(col, new GUIContent("Col"));

            square = (Texture)Resources.Load("tile-test");
            float width = square.width;
            float height = square.height;

            for (int x = 0; x < row.intValue; x++) {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                for (int y = 0; y < col.intValue; y++) {
                    GUILayout.Box(square);
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            serializedObject.ApplyModifiedProperties();
        }
#endif
    }
}