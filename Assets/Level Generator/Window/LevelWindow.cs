using System.Collections.Generic;
using EditorAttributes;
using UnityEditor;
using UnityEngine;

namespace LEVEL_GENERATOR.WINDOW {
    public class LevelWindow : EditorWindow {
        [field: SerializeField] public int Row { get; private set; } = 2;
        [field: SerializeField] public int Column { get; private set; } = 2;
        [field: SerializeField] public List<SquareRow> squareStates = new List<SquareRow>();


        [MenuItem("Window/Level Generator")]
        public static void ShowWindow() {
            GetWindow<LevelWindow>("Level Generator");
        }

        void OnGUI() {
            EnsureSquareStatesInitialized();

            GUILayout.Label("Level Generator", EditorStyles.boldLabel);

            DrawRowColumnFields();

            GUILayout.FlexibleSpace();

            UpdateSquareStatesIfNeeded();

            DrawSquaresGrid();

            GUILayout.FlexibleSpace();

            DrawCreateLevelButton();
        }
        /// <summary> Garante que a lista squareStates está inicializada corretamente e corresponde aos valores atuais de Row e Column. </summary>
        private void EnsureSquareStatesInitialized() {
            // Certifica se de que squareStates está inicializado corretamente
            if (squareStates == null) squareStates = new List<SquareRow>();

            // Ajusta número de linhas
            while (squareStates.Count < Row) squareStates.Add(new SquareRow());
            while (squareStates.Count > Row) squareStates.RemoveAt(squareStates.Count - 1);

            // Ajusta número de colunas em cada linha
            for (int x = 0; x < Row; x++) {
                var row = squareStates[x];
                if (row.squares == null) row.squares = new List<SquareStates>();
                while (row.squares.Count < Column) row.squares.Add(SquareStates.Empty);
                while (row.squares.Count > Column) row.squares.RemoveAt(row.squares.Count - 1);
            }
        }

        /// <summary> Desenha os campos de entrada para Row e Column. </summary>
        private void DrawRowColumnFields() {
            Row = EditorGUILayout.IntField("Row", Row);
            if (Row < 2) Row = 2;

            Column = EditorGUILayout.IntField("Column", Column);
            if (Column < 2) Column = 2;

            if (GUILayout.Button("Reset")) ResetWindow();
        }

        /// <summary> Atualiza os estados dos quadrados se Row ou Column forem alterados. </summary>
        private void UpdateSquareStatesIfNeeded() {
            bool isRowCountDifferent = squareStates.Count != Row;
            bool isColumnCountDifferent = Row > 0 && squareStates[0].squares.Count != Column;

            // Reinicializa a lista se o número de linhas ou colunas mudou
            if (isRowCountDifferent || isColumnCountDifferent) {
                squareStates = new List<SquareRow>();

                for (int i = 0; i < Row; i++) {
                    squareStates.Add(new SquareRow());

                    for (int j = 0; j < Column; j++)
                        squareStates[i].squares.Add(SquareStates.Empty);

                }
            }
        }

        /// <summary> Obtém a cor para uma opção específica de SquareOption. </summary>
        private Color GetColorForOption(SquareStates option) {
            switch (option) {
                case SquareStates.Empty: return Color.white;
                case SquareStates.Structure: return Color.yellow;
                case SquareStates.Enemy: return Color.red;
                default: return Color.white;
            }
        }

        /// <summary> Desenha a grade de quadrados baseada nos valores atuais de Row e Column. </summary>
        private void DrawSquaresGrid() {
            float width = 40;
            float height = 40;

            // Desenha a grade de quadrados
            for (int x = 0; x < Row; x++) {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int y = 0; y < Column; y++) {
                    Color choiceColor = GetColorForOption(squareStates[x].squares[y]);
                    Color prevColor = GUI.backgroundColor;
                    GUI.backgroundColor = choiceColor;
                    if (GUILayout.Button("", GUILayout.Width(width), GUILayout.Height(height)))
                        squareStates[x].squares[y] = (SquareStates)(((int)squareStates[x].squares[y] + 1) % 3);
                    GUI.backgroundColor = prevColor;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }

        /// <summary> Desenha o botão para criar os dados do nível. </summary>
        private void DrawCreateLevelButton() {
            if (GUILayout.Button("Create Level Data"))
                CreateSerializedObject();
        }

        /// <summary> Reseta a janela para os valores padrão. </summary>
        private void ResetWindow() {
            Row = 2;
            Column = 2;
            squareStates = new List<SquareRow>(2);
            for (int x = 0; x < Row; x++) {
                squareStates.Add(new SquareRow());

                for (int y = 0; y < Column; y++)
                    squareStates[x].squares.Add(SquareStates.Empty);
            }
        }

        /// <summary> Cria e salva um ScriptableObject LevelData baseado nos estados atuais dos quadrados. </summary>
        private void CreateSerializedObject() {
            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

            levelData.Squares = new List<SquareRow>();

            // Copia os estados dos quadrados para o novo LevelData
            foreach (var row in squareStates) {
                var newRow = new SquareRow();
                newRow.squares.AddRange(row.squares);
                levelData.Squares.Add(newRow);
            }

            string path = EditorUtility.SaveFilePanelInProject("Save Level Data", "New Level Data", "asset", "Please enter a file name to save the level data to");

            if (string.IsNullOrEmpty(path)) return;

            AssetDatabase.CreateAsset(levelData, path);
            AssetDatabase.SaveAssets();

            Debug.Log("Level Data asset foi criado com sucesso!");
            Debug.Log("Foi criado o asset em: " + path);
        }
    }
}