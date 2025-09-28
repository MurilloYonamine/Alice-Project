using UnityEditor;
using UnityEngine;
using System.Linq;
using ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE;

namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR  {
#if UNITY_EDITOR
    public class CellTypeCreatorWindow : EditorWindow {
        private string _typeName = "Novo Tipo";
        private Color _color = Color.white;
        private Sprite _icon;
        private System.Type[] _derivedTypes;
        private string[] _typeNames;
        private int _typeIndex = 0;

        [MenuItem("Window/CellType Creator")]
        public static void ShowWindow() {
            GetWindow<CellTypeCreatorWindow>("Criar Novo CellTypeSO");
        }

        void OnEnable() {
            // Detecta todos os tipos derivados de CellTypeSO
            var baseType = typeof(CellTypeSO);
            _derivedTypes = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(baseType) && !type.IsAbstract)
                .ToArray();
            _typeNames = _derivedTypes.Select(t => t.Name).ToArray();
        }

        void OnGUI() {
            GUILayout.Label("Criar Novo Tipo de Célula", EditorStyles.boldLabel);
            _typeName = EditorGUILayout.TextField("Nome do Tipo", _typeName);
            _color = EditorGUILayout.ColorField("Cor", _color);
            _icon = (Sprite)EditorGUILayout.ObjectField("Ícone", _icon, typeof(Sprite), false);
            if (_typeNames == null || _typeNames.Length == 0) {
                EditorGUILayout.HelpBox("Nenhum tipo derivado de CellTypeSO encontrado.", MessageType.Warning);
                return;
            }
            _typeIndex = EditorGUILayout.Popup("Tipo Base", _typeIndex, _typeNames);

            if (GUILayout.Button("Criar CellTypeSO")) {
                CreateCellTypeSO();
                ReloadTypes();
            }
        }
        private void ReloadTypes() {
            var baseType = typeof(CellTypeSO);
            _derivedTypes = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(baseType) && !type.IsAbstract)
                .ToArray();
            _typeNames = _derivedTypes.Select(t => t.Name).ToArray();
            Repaint();
        }

        private void CreateCellTypeSO() {
            string assetPath = "Assets/_MAIN/Resources/CellTypes/" + _typeName + ".asset";
            if (_derivedTypes == null || _derivedTypes.Length == 0) return;
            var selectedType = _derivedTypes[_typeIndex];
            CellTypeSO newCellType = ScriptableObject.CreateInstance(selectedType) as CellTypeSO;
            if (newCellType != null) {
                newCellType.typeName = _typeName;
                newCellType.color = _color;
                newCellType.icon = _icon;
                AssetDatabase.CreateAsset(newCellType, assetPath);
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = newCellType;
                Debug.Log("CellTypeSO criado em: " + assetPath);
            }
        }
    }
#endif
}
