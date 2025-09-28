using UnityEngine;
using UnityEngine.Tilemaps;

namespace ALICE_PROJECT.LEVELGENERATOR.EDITOR.CORE {

    [CreateAssetMenu(menuName = "LevelGenerator/CellTypeSO")]
    public abstract class CellTypeSO : ScriptableObject {
        public string typeName;
        public Color color = Color.white;
        public Sprite icon;
        public abstract void Execute(ICellTypeContext context);
    }
}
