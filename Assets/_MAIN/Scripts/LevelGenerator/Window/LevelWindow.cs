using System.Collections.Generic;
using System.Linq;
using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE;
using ALICE_PROJECT.LEVELGENERATOR.EDITOR.STRATEGY;
using UnityEditor;
using UnityEngine;

namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR {
#if UNITY_EDITOR
    public class LevelWindow : EditorWindow {
        [field: SerializeField] public int ROW = 23;
        [field: SerializeField] public int COLUMN = 10;
        [field: SerializeField] public List<LevelRow> levelRows = new List<LevelRow>();

        [field: SerializeField] private CellTypeSO _leftClick;
        [field: SerializeField] private CellTypeSO _rightClick;
        private List<CellTypeSO> _cellTypes;

        [MenuItem("Window/Level Generator")]
        public static void ShowWindow() {
            GetWindow<LevelWindow>("Level Generator");
        }

        void OnEnable() {
            LoadCellTypes();
        }

        void OnFocus() {
            LoadCellTypes();
        }

        void OnGUI() {
            EnsureCellTypeInitialized();
            LevelWindowVisuals.DrawAnimatedTitle("Level Generator");
            GUILayout.Space(10);

            LevelWindowVisuals.DrawRowColumnFields(ROW, COLUMN);
            GUILayout.Space(10);

            DrawCellTypeSelectors();
            GUILayout.Space(10);

            LevelWindowVisuals.DrawActionButtons(ResetWindow, CreateSerializedObject);
            GUILayout.Space(10);

            LevelWindowVisuals.DrawTestButton(GenerateATestLevel);
            GUILayout.Space(20);
            DrawSquaresGrid();
        }

        private void LoadCellTypes() {
            _cellTypes = Resources.LoadAll<CellTypeSO>("CellTypes").ToList();
            if (_cellTypes.Count > 0) {
                if (_leftClick == null) _leftClick = _cellTypes[0];
                if (_rightClick == null) _rightClick = _cellTypes[0];
            }
        }

        private void EnsureCellTypeInitialized() {
            if (levelRows == null) levelRows = new List<LevelRow>();
            while (levelRows.Count < ROW) levelRows.Add(new LevelRow());
            while (levelRows.Count > ROW) levelRows.RemoveAt(levelRows.Count - 1);
            // Find the EmptyCellTypeSO asset from loaded cell types
            CellTypeSO emptyType = _cellTypes?.FirstOrDefault(t => t is EmptyCellTypeSO);
            for (int x = 0; x < ROW; x++) {
                var row = levelRows[x];
                if (row.rowElements == null) row.rowElements = new List<CellTypeSO>();
                while (row.rowElements.Count < COLUMN) row.rowElements.Add(emptyType);
                while (row.rowElements.Count > COLUMN) row.rowElements.RemoveAt(row.rowElements.Count - 1);
            }
        }

        private void DrawCellTypeSelectors() {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Clique Esquerdo", GUILayout.Width(120));
            int leftIndex = Mathf.Max(0, _cellTypes.IndexOf(_leftClick));
            if (leftIndex >= _cellTypes.Count) leftIndex = 0;
            leftIndex = EditorGUILayout.Popup(leftIndex, _cellTypes.Select(t => t.typeName).ToArray(), GUILayout.Width(150));
            _leftClick = _cellTypes.Count > 0 ? _cellTypes[leftIndex] : null;
            GUILayout.Space(40);
            GUILayout.Label("Clique Direito", GUILayout.Width(120));
            int rightIndex = Mathf.Max(0, _cellTypes.IndexOf(_rightClick));
            if (rightIndex >= _cellTypes.Count) rightIndex = 0;
            rightIndex = EditorGUILayout.Popup(rightIndex, _cellTypes.Select(t => t.typeName).ToArray(), GUILayout.Width(150));
            _rightClick = _cellTypes.Count > 0 ? _cellTypes[rightIndex] : null;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        private void ResetWindow() {
            // Find the EmptyCellTypeSO asset from loaded cell types
            CellTypeSO emptyType = _cellTypes?.FirstOrDefault(t => t is EmptyCellTypeSO);
            levelRows = new List<LevelRow>();
            for (int x = 0; x < ROW; x++) {
                levelRows.Add(new LevelRow());
                for (int y = 0; y < COLUMN; y++)
                    levelRows[x].rowElements.Add(emptyType);
            }
        }

        private void GenerateATestLevel() {
            ResetWindow();
            System.Random rand = new System.Random();
            int lastStructureRow = -5;
            const int FIRST_ROW = 2;
            for (int x = FIRST_ROW; x < ROW; x++) {
                if (x - lastStructureRow < 3) continue;
                int structureLength = rand.Next(1, 5);
                int startY = rand.Next(0, COLUMN - structureLength + 1);
                // Coloca a estrutura
                for (int y = startY; y < startY + structureLength; y++) {
                    levelRows[x].rowElements[y] = _cellTypes.FirstOrDefault(t => t.typeName == "Ground") ?? _cellTypes[0];
                }
                // Coloca apenas 1 inimigo acima da estrutura
                if (x > 0) {
                    int enemyY = rand.Next(startY, startY + structureLength);
                    if (levelRows[x - 1].rowElements[enemyY] == _cellTypes[0]) {
                        levelRows[x - 1].rowElements[enemyY] = _cellTypes.FirstOrDefault(t => t.typeName == "Enemy") ?? _cellTypes[0];
                    }
                }
                lastStructureRow = x;
            }
        }

        private void CreateSerializedObject() {
            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
            levelData.Cells = new List<LevelRow>();
            // Find the EmptyCellTypeSO asset from loaded cell types
            CellTypeSO emptyType = _cellTypes?.FirstOrDefault(t => t is EmptyCellTypeSO);
            foreach (LevelRow row in levelRows) {
                LevelRow newRow = new LevelRow();
                foreach (var cell in row.rowElements) {
                    newRow.rowElements.Add(cell ?? emptyType);
                }
                levelData.Cells.Add(newRow);
            }
            const string LEVELS_PATH = "Assets/_MAIN/Resources/Levels";
            string path = EditorUtility.SaveFilePanelInProject("Save Level Data", "New Level Data", "asset", "Please enter a file name to save the level data to", LEVELS_PATH);
            if (string.IsNullOrEmpty(path)) return;
            AssetDatabase.CreateAsset(levelData, path);
            AssetDatabase.SaveAssets();
            ResetWindow();
            Debug.Log("Level Data asset foi criado com sucesso!");
            Debug.Log("Foi criado o asset em: " + path);
        }


        private void DrawSquaresGrid() {
            float width = 24;
            float height = 24;
            for (int x = 0; x < ROW; x++) {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int y = 0; y < COLUMN; y++) {
                    Color choiceColor = levelRows[x].rowElements[y] != null ? levelRows[x].rowElements[y].color : Color.white;
                    Color prevColor = GUI.backgroundColor;
                    GUI.backgroundColor = choiceColor;
                    Rect buttonRect = GUILayoutUtility.GetRect(width, height);
                    if (GUI.Button(buttonRect, GUIContent.none)) {
                        bool isRightButton = Event.current.button == 1;
                        UpdateSquareState(x, y, isRightButton);
                    }
                    GUI.backgroundColor = prevColor;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }

        private void UpdateSquareState(int x, int y, bool isRightClick = false) {
            CellTypeSO mouseClick = isRightClick ? _rightClick : _leftClick;
            levelRows[x].rowElements[y] = mouseClick;
        }
    }
#endif
}
