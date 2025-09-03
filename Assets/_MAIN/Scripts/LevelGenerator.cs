using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Create New Level", menuName = "Level Generation")]
public class LevelGenerator : ScriptableObject {
    [field: SerializeField] public int Row;
    [field: SerializeField] public int Col;

    [field: SerializeField] public Tilemap tilemap;
}
[CustomEditor(typeof(LevelGenerator))]
public class LevelEditor : Editor {
    SerializedProperty tilemap;
    SerializedProperty Row;
    SerializedProperty Col;

    private Texture square;

    private void OnEnable() {
        tilemap = serializedObject.FindProperty("tilemap");
        Row = serializedObject.FindProperty("Row");
        Col = serializedObject.FindProperty("Col");
    }

#if UNITY_EDITOR
    public override void OnInspectorGUI() {
        serializedObject.Update();

        LevelGenerator levelGenerator = (LevelGenerator)target;

        // Exibe as propriedades no Inspetor
        EditorGUILayout.PropertyField(tilemap, new GUIContent("Tilemap"));
        EditorGUILayout.PropertyField(Row, new GUIContent("Row"));
        EditorGUILayout.PropertyField(Col, new GUIContent("Col"));

        square = (Texture)Resources.Load("tile-test");
        float width = square.width;
        float height = square.height;

        for (int x = 0; x <= Row.intValue; x++) {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            for (int y = 0; y <= Col.intValue; y++) {
                if (x == 0 && y == 0) {
                    GUILayout.Label(""); // Empty corner
                } else if (x == 0) {
                    GUILayout.Label((y - 1).ToString(), GUILayout.Width(square.width)); // Horizontal numbers
                } else if (y == 0) {
                    GUILayout.Label((x - 1).ToString(), GUILayout.Height(square.height)); // Vertical numbers
                } else {
                    GUILayout.Box(square);
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        serializedObject.ApplyModifiedProperties();
    }
#endif
}