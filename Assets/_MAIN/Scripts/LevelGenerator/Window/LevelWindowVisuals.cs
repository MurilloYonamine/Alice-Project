using UnityEditor;
using UnityEngine;
using ALICE_PROJECT.LEVELGENERATOR.DATA;

namespace ALICE_PROJECT.LEVELGENERATOR.WINDOW {
    public static class LevelWindowVisuals {
        public static void DrawAnimatedTitle(string title) {
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel) {
                fontSize = 48,
                alignment = TextAnchor.MiddleCenter
            };
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
        }

        public static void DrawRowColumnFields(int row, int column) {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.IntField("Número de Linhas:", row, GUILayout.Width(200));
            GUILayout.Space(20);
            EditorGUILayout.IntField("Número de Colunas:", column, GUILayout.Width(200));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

    public static void DrawClickTypeSelectors(ref CellType leftClick, ref CellType rightClick) {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Botão Esquerdo:", GUILayout.Width(100));
            leftClick = (CellType)EditorGUILayout.EnumPopup(leftClick, GUILayout.Width(100));
            GUILayout.Space(20);
            GUILayout.Label("Botão Direito:", GUILayout.Width(100));
            rightClick = (CellType)EditorGUILayout.EnumPopup(rightClick, GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void DrawActionButtons(System.Action resetAction, System.Action createDataAction) {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Resetar Level", GUILayout.Width(250))) resetAction?.Invoke();
            if (GUILayout.Button("Criar Dados do Level", GUILayout.Width(250))) createDataAction?.Invoke();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void DrawTestButton(System.Action testAction) {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Gerar Level de Teste", GUILayout.Width(250))) testAction?.Invoke();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
