using System.Collections.Generic;
using EditorAttributes;
using UnityEditor;
using UnityEngine;

namespace LEVEL_GENERATOR.WINDOW {
    public class LevelWindow : EditorWindow {
        [field: SerializeField] public int Row { get; private set; } = 2;
        [field: SerializeField] public int Column { get; private set; } = 2;
        [field: SerializeField] public List<SquareRow> squareStates = new List<SquareRow>();

        [field: SerializeField] private SquareStates _leftClick = SquareStates.Structure;
        [field: SerializeField] private SquareStates _rightClick = SquareStates.Empty;

        [MenuItem("Window/Level Generator")]
        public static void ShowWindow() {
            GetWindow<LevelWindow>("Level Generator");
        }

        void OnGUI() {
            EnsureSquareStatesInitialized();

            DrawLevelConfigurationFields();

            GUILayout.Space(20);

            UpdateSquareStatesIfNeeded();

            DrawSquaresGrid();

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
        private void DrawLevelConfigurationFields() {
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel) {
                fontSize = 48,
                alignment = TextAnchor.MiddleCenter
            };

            string title = "Level Generator";
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            for (int i = 0; i < title.Length; i++) {
                Color prevColor = GUI.color;
                GUI.color = Color.HSVToRGB((Time.realtimeSinceStartup + i * 0.1f) % 1, 1, 1);
                GUILayout.Label(title[i].ToString(), titleStyle);
                GUI.color = prevColor;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Row = EditorGUILayout.IntField("Número de Linhas:", Row, GUILayout.Width(200));
            if (Row < 2) Row = 2;
            GUILayout.Space(20);
            Column = EditorGUILayout.IntField("Número de Colunas:", Column, GUILayout.Width(200));
            if (Column < 2) Column = 2;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Botão Esquerdo:", GUILayout.Width(100));
            _leftClick = (SquareStates)EditorGUILayout.EnumPopup(_leftClick, GUILayout.Width(100));
            GUILayout.Space(20);
            GUILayout.Label("Botão Direito:", GUILayout.Width(100));
            _rightClick = (SquareStates)EditorGUILayout.EnumPopup(_rightClick, GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Resetar Level", GUILayout.Width(250))) ResetWindow();
            if (GUILayout.Button("Criar Dados do Level", GUILayout.Width(250))) CreateSerializedObject();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
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
            float width = 24;
            float height = 24;

            // Desenha a grade de quadrados
            for (int x = 0; x < Row; x++) {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int y = 0; y < Column; y++) {

                    Color choiceColor = GetColorForOption(squareStates[x].squares[y]);
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

        private SquareStates UpdateSquareState(int x, int y, bool isRightClick = false) {
            SquareStates mouseClick = isRightClick ? _rightClick : _leftClick;

            switch (mouseClick) {
                case SquareStates.Empty: squareStates[x].squares[y] = SquareStates.Empty; break;
                case SquareStates.Structure: squareStates[x].squares[y] = SquareStates.Structure; break;
                case SquareStates.Enemy: squareStates[x].squares[y] = SquareStates.Enemy; break;
                default: squareStates[x].squares[y] = SquareStates.Empty; break;
            }

            return mouseClick;
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

            const string LEVELS_PATH = "Assets/_MAIN/Resources/Levels";
            string path = EditorUtility.SaveFilePanelInProject("Save Level Data", "New Level Data", "asset", "Please enter a file name to save the level data to", LEVELS_PATH);

            if (string.IsNullOrEmpty(path)) return;

            AssetDatabase.CreateAsset(levelData, path);
            AssetDatabase.SaveAssets();

            Debug.Log("Level Data asset foi criado com sucesso!");
            Debug.Log("Foi criado o asset em: " + path);
        }
    }
}