using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LEVEL {
    [CustomEditor(typeof(LevelGenerator))]

    public class LevelEditor : Editor {
        SerializedProperty row;
        SerializedProperty col;

        private Texture square;

        private void OnEnable() {
            row = serializedObject.FindProperty("_row");
            col = serializedObject.FindProperty("_col");
        }

#if UNITY_EDITOR
        public override void OnInspectorGUI() {
            serializedObject.Update();

            LevelGenerator levelGenerator = (LevelGenerator)target;

            // Exibe as propriedades no Inspetor
            EditorGUILayout.PropertyField(row, new GUIContent("Row"));
            EditorGUILayout.PropertyField(col, new GUIContent("Col"));

            square = (Texture)Resources.Load("tile-test");
            float width = 56;
            float height = 56;

            List<bool> buttonList = new List<bool>();

            for (int x = 0; x < row.intValue; x++) {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                for (int y = 0; y < col.intValue; y++) {
                    bool button = GUILayout.Button(square, GUILayout.Width(width), GUILayout.Height(height));
                    buttonList.Add(button);
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            foreach (bool button in buttonList) {
                if (button)
                    Debug.Log("Bazinga");
            }


            serializedObject.ApplyModifiedProperties();
        }
#endif
    }
}