using System.Collections.Generic;
using EditorAttributes;
using UnityEditor;
using UnityEngine;

public class LevelWindow : EditorWindow {
    [field: SerializeField] public int Row { get; private set; }
    [field: SerializeField] public int Column { get; private set; }

    private Texture squareTexture;

    [MenuItem("Window/Level Generator")]
    public static void ShowWindow() {
        GetWindow<LevelWindow>("Level Generator");
    }

    private List<List<SquareStates>> squareStates = new List<List<SquareStates>>();

    void OnGUI() {
        GUILayout.Label("Level Generator", EditorStyles.boldLabel);

        DrawRowColumnFields();

        GUILayout.FlexibleSpace();

        UpdateSquareStatesIfNeeded();

        DrawSquaresGrid();

        GUILayout.FlexibleSpace();

        DrawCreateLevelButton();
    }

    /// <summary> Draws the input fields for Row and Column. </summary>
    private void DrawRowColumnFields() {
        Row = EditorGUILayout.IntField("Row", Row);
        Column = EditorGUILayout.IntField("Column", Column);
    }

    /// <summary> Updates the square states if the Row or Column has changed. </summary>
    private void UpdateSquareStatesIfNeeded() {
        bool isRowCountDifferent = squareStates.Count != Row;
        bool isColumnCountDifferent = Row > 0 && squareStates.Count > 0 && squareStates[0].Count != Column;

        // If the number of rows or columns has changed, reinitialize the square states.
        if (isRowCountDifferent || isColumnCountDifferent) {
            squareStates.Clear();

            for (int x = 0; x < Row; x++) {
                List<SquareStates> rowList = new List<SquareStates>();

                for (int y = 0; y < Column; y++)
                    rowList.Add(SquareStates.Empty);

                squareStates.Add(rowList);
            }
        }
    }

    /// <summary> Gets the color for a specific SquareOption. </summary>
    private Color GetColorForOption(SquareStates option) {
        switch (option) {
            case SquareStates.Empty: return Color.white;
            case SquareStates.Structure: return Color.yellow;
            case SquareStates.Enemy: return Color.red;
            default: return Color.white;
        }
    }

    /// <summary> Draws the grid of squares based on the current Row and Column values. </summary>
    private void DrawSquaresGrid() {
        float width = 40;
        float height = 40;

        for (int x = 0; x < Row; x++) {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            for (int y = 0; y < Column; y++) {
                // Get the color based on the current state of the square.
                Color choiceColor = GetColorForOption(squareStates[x][y]);
                Color prevColor = GUI.backgroundColor;
                GUI.backgroundColor = choiceColor;

                // Draw the button for the square and Cycle through the options.
                if (GUILayout.Button("", GUILayout.Width(width), GUILayout.Height(height)))
                    squareStates[x][y] = (SquareStates)(((int)squareStates[x][y] + 1) % 3);

                GUI.backgroundColor = prevColor;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    /// <summary> Draws the button to create level data. </summary>
    private void DrawCreateLevelButton() {
        if (GUILayout.Button("Create Level Data"))
            CreateSerializedObject();
    }
    private void CreateSerializedObject() {
        string rowString = "All squares states:";
        foreach (var row in squareStates) {
            rowString += "\n[  ";
            
            foreach (var s in row)
                rowString += s.ToString()[0] + "   ";
            
            rowString += "]";
        }

        Debug.Log(rowString);

        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

        levelData.squares = squareStates;

        string path = EditorUtility.SaveFilePanelInProject("Save Level Data", "New Level Data", "asset", "Please enter a file name to save the level data to");

        if (string.IsNullOrEmpty(path))
            return;

        AssetDatabase.CreateAsset(levelData, path);
        AssetDatabase.SaveAssets();

        Debug.Log("Create Level Data");
    }
}

