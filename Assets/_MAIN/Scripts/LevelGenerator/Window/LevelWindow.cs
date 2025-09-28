using System.Collections.Generic;
using CellType = ALICE_PROJECT.LEVELGENERATOR.DATA.CellType;
using ALICE_PROJECT.LEVELGENERATOR.DATA;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace ALICE_PROJECT.LEVELGENERATOR.WINDOW {
#if UNITY_EDITOR
    public class LevelWindow : EditorWindow {
        [field: SerializeField, ReadOnly] public const int ROW = 23;
        [field: SerializeField, ReadOnly] public const int COLUMN = 10;
        [field: SerializeField] public List<LevelRow> levelRows = new List<LevelRow>();

        [field: SerializeField] private CellType _leftClick = CellType.Ground;
        [field: SerializeField] private CellType _rightClick = CellType.Ground;

        [MenuItem("Window/Level Generator")]
        public static void ShowWindow() {
            GetWindow<LevelWindow>("Level Generator");
        }

        void OnGUI() {
            EnsureCellTypeInitialized();

            DrawLevelConfigurationFields();

            GUILayout.Space(20);

            UpdateCellTypeIfNeeded();

            DrawSquaresGrid();

        }
        /// <summary> Garante que a lista CellType está inicializada corretamente e corresponde aos valores atuais de Row e Column. </summary>
        private void EnsureCellTypeInitialized() {
            // Certifica se de que CellType está inicializado corretamente
            if (levelRows == null) levelRows = new List<LevelRow>();

            // Ajusta número de linhas
            while (levelRows.Count < ROW) levelRows.Add(new LevelRow());
            while (levelRows.Count > ROW) levelRows.RemoveAt(levelRows.Count - 1);

            // Ajusta número de colunas em cada linha
            for (int x = 0; x < ROW; x++) {
                var row = levelRows[x];
                if (row.rowElements == null) row.rowElements = new List<CellType>();
                while (row.rowElements.Count < COLUMN) row.rowElements.Add(CellType.Empty);
                while (row.rowElements.Count > COLUMN) row.rowElements.RemoveAt(row.rowElements.Count - 1);
            }
        }

        /// <summary> Desenha os campos de entrada para Row e Column. </summary>
        private void DrawLevelConfigurationFields() {
            LevelWindowVisuals.DrawAnimatedTitle("Level Generator");
            GUILayout.Space(10);
            LevelWindowVisuals.DrawRowColumnFields(ROW, COLUMN);
            GUILayout.Space(10);
            LevelWindowVisuals.DrawClickTypeSelectors(ref _leftClick, ref _rightClick);
            GUILayout.Space(10);
            LevelWindowVisuals.DrawActionButtons(ResetWindow, CreateSerializedObject);
            GUILayout.Space(10);
            LevelWindowVisuals.DrawTestButton(GenerateATestLevel);
        }

        /// <summary> Atualiza os estados dos quadrados se Row ou Column forem alterados. </summary>
        private void UpdateCellTypeIfNeeded() {
            bool isRowCountDifferent = levelRows.Count != ROW;
            bool isColumnCountDifferent = ROW > 0 && levelRows[0].rowElements.Count != COLUMN;

            // Reinicializa a lista se o número de linhas ou colunas mudou
            if (isRowCountDifferent || isColumnCountDifferent) {
                levelRows = new List<LevelRow>();

                for (int i = 0; i < ROW; i++) {
                    levelRows.Add(new LevelRow());

                    for (int j = 0; j < COLUMN; j++)
                        levelRows[i].rowElements.Add(CellType.Empty);

                }
            }
        }

        /// <summary> Obtém a cor para uma opção específica de SquareOption. </summary>
        private Color GetColorForOption(CellType option) {
            switch (option) {
                case CellType.Empty: return Color.white;
                case CellType.Ground: return Color.yellow;
                case CellType.Enemy: return Color.red;
                default: return Color.white;
            }
        }

        /// <summary> Desenha a grade de quadrados baseada nos valores atuais de Row e Column. </summary>
        private void DrawSquaresGrid() {
            float width = 24;
            float height = 24;

            // Desenha a grade de quadrados
            for (int x = 0; x < ROW; x++) {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int y = 0; y < COLUMN; y++) {

                    Color choiceColor = GetColorForOption(levelRows[x].rowElements[y]);
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

        private CellType UpdateSquareState(int x, int y, bool isRightClick = false) {
            CellType mouseClick = isRightClick ? _rightClick : _leftClick;

            switch (mouseClick) {
                case CellType.Empty: levelRows[x].rowElements[y] = CellType.Empty; break;
                case CellType.Ground: levelRows[x].rowElements[y] = CellType.Ground; break;
                case CellType.Enemy: levelRows[x].rowElements[y] = CellType.Enemy; break;
                default: levelRows[x].rowElements[y] = CellType.Empty; break;
            }

            return mouseClick;
        }

        /// <summary> Reseta a janela para os valores padrão. </summary>
        private void ResetWindow() {
            // ROW = 23;
            // COLUMN = 10;
            levelRows = new List<LevelRow>(2);
            for (int x = 0; x < ROW; x++) {
                levelRows.Add(new LevelRow());

                for (int y = 0; y < COLUMN; y++)
                    levelRows[x].rowElements.Add(CellType.Empty);
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
                    levelRows[x].rowElements[y] = CellType.Ground;
                }

                // Coloca apenas 1 inimigo acima da estrutura
                if (x > 0) {
                    int enemyY = rand.Next(startY, startY + structureLength);
                    if (levelRows[x - 1].rowElements[enemyY] == CellType.Empty) {
                        levelRows[x - 1].rowElements[enemyY] = CellType.Enemy;
                    }
                }

                lastStructureRow = x;
            }
        }

        /// <summary> Cria e salva um ScriptableObject LevelData baseado nos estados atuais dos quadrados. </summary>
        private void CreateSerializedObject() {
            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

            levelData.Cells = new List<LevelRow>();

            // Copia os estados dos quadrados para o novo LevelData
            foreach (var row in levelRows) {
                var newRow = new LevelRow();
                newRow.rowElements.AddRange(row.rowElements);
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
    }
#endif
}